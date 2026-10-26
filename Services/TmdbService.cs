using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using CineAPI.Models;

namespace CineAPI.Services;

public sealed class TmdbService(
    HttpClient httpClient,
    IConfiguration configuration,
    ILogger<TmdbService> logger) : ITmdbService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyList<Movie>> SearchMoviesAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(
            $"search/movie?query={Uri.EscapeDataString(query)}&language=es-MX&include_adult=false",
            cancellationToken);

        var result = await DeserializeAsync<TmdbSearchResponse>(response, cancellationToken);
        return result.Results.Select(ToMovie).ToList();
    }

    public async Task<MovieDetails?> GetMovieDetailsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync($"movie/{id}?language=es-MX", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        var result = await DeserializeAsync<TmdbMovieDetailsResponse>(response, cancellationToken);
        return new MovieDetails
        {
            Id = result.Id,
            Title = result.Title ?? "Título no disponible",
            Overview = result.Overview ?? "Sinopsis no disponible.",
            ReleaseDate = result.ReleaseDate ?? string.Empty,
            VoteAverage = result.VoteAverage,
            PosterPath = result.PosterPath,
            Runtime = result.Runtime,
            BackdropPath = result.BackdropPath,
            Genres = result.Genres
                .Where(genre => !string.IsNullOrWhiteSpace(genre.Name))
                .Select(genre => genre.Name)
                .ToList()
        };
    }

    private async Task<HttpResponseMessage> SendAsync(
        string relativeUrl,
        CancellationToken cancellationToken)
    {
        var token = configuration["Tmdb:ReadAccessToken"];
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new TmdbServiceException(
                "El servicio de películas no está configurado. Añade el token de TMDB mediante User Secrets.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("TMDB respondió con el estado HTTP {StatusCode}.", response.StatusCode);
                throw new TmdbServiceException(
                    response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden
                        ? "La credencial de TMDB no es válida."
                        : "TMDB no pudo completar la solicitud.");
            }

            return response;
        }
        catch (TmdbServiceException)
        {
            throw;
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "No se pudo conectar con TMDB.");
            throw new TmdbServiceException(
                "No se pudo conectar con el servicio de películas.", exception);
        }
    }

    private static async Task<T> DeserializeAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
            return result ?? throw new TmdbServiceException("La respuesta de TMDB estaba vacía.");
        }
        catch (JsonException exception)
        {
            throw new TmdbServiceException(
                "La respuesta del servicio de películas no tenía un formato válido.", exception);
        }
    }

    private static Movie ToMovie(TmdbMovieResponse movie) => new()
    {
        Id = movie.Id,
        Title = string.IsNullOrWhiteSpace(movie.Title) ? "Título no disponible" : movie.Title,
        Overview = string.IsNullOrWhiteSpace(movie.Overview)
            ? "Sinopsis no disponible."
            : movie.Overview,
        ReleaseDate = movie.ReleaseDate ?? string.Empty,
        VoteAverage = movie.VoteAverage,
        PosterPath = movie.PosterPath
    };
}

public sealed class TmdbServiceException(string message, Exception? innerException = null)
    : Exception(message, innerException);
