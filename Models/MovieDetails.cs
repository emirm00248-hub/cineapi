namespace CineAPI.Models;

public sealed class MovieDetails : Movie
{
    public int? Runtime { get; set; }
    public string? BackdropPath { get; set; }
    public List<string> Genres { get; set; } = [];

    public string? BackdropUrl => string.IsNullOrWhiteSpace(BackdropPath)
        ? null
        : $"https://image.tmdb.org/t/p/w1280{BackdropPath}";

    public string RuntimeText => Runtime is > 0
        ? $"{Runtime} min"
        : "Duración no disponible";
}

