using CineAPI.Models;

namespace CineAPI.Services;

public interface ITmdbService
{
    Task<IReadOnlyList<Movie>> SearchMoviesAsync(string query, CancellationToken cancellationToken = default);
    Task<MovieDetails?> GetMovieDetailsAsync(int id, CancellationToken cancellationToken = default);
}

