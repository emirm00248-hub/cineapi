using System.Text.Json.Serialization;

namespace CineAPI.Models;

public sealed class TmdbSearchResponse
{
    [JsonPropertyName("results")]
    public List<TmdbMovieResponse> Results { get; set; } = [];
}

public class TmdbMovieResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }
}

public sealed class TmdbMovieDetailsResponse : TmdbMovieResponse
{
    [JsonPropertyName("runtime")]
    public int? Runtime { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("genres")]
    public List<TmdbGenreResponse> Genres { get; set; } = [];
}

public sealed class TmdbGenreResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
