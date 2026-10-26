namespace CineAPI.Models;

public sealed class MovieSearchViewModel
{
    public string Query { get; set; } = string.Empty;
    public IReadOnlyList<Movie> Movies { get; set; } = [];
    public string? ErrorMessage { get; set; }
    public bool HasSearched => !string.IsNullOrWhiteSpace(Query);
}

