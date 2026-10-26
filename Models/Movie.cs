namespace CineAPI.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public string ReleaseDate { get; set; } = string.Empty;
    public double VoteAverage { get; set; }
    public string? PosterPath { get; set; }

    public string? PosterUrl => string.IsNullOrWhiteSpace(PosterPath)
        ? null
        : $"https://image.tmdb.org/t/p/w500{PosterPath}";

    public string DisplayReleaseDate => DateTime.TryParse(ReleaseDate, out var date)
        ? date.ToString("dd/MM/yyyy")
        : "Fecha no disponible";
}
