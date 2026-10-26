using CineAPI.Models;
using CineAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineAPI.Controllers;

public sealed class MoviesController(
    ITmdbService tmdbService,
    ILogger<MoviesController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(
        string? query,
        CancellationToken cancellationToken)
    {
        var model = new MovieSearchViewModel { Query = query?.Trim() ?? string.Empty };
        if (string.IsNullOrWhiteSpace(model.Query))
        {
            return View(model);
        }

        try
        {
            model.Movies = await tmdbService.SearchMoviesAsync(model.Query, cancellationToken);
            if (model.Movies.Count == 0)
            {
                model.ErrorMessage = "No encontramos películas con ese nombre.";
            }
        }
        catch (TmdbServiceException exception)
        {
            logger.LogWarning(exception, "La búsqueda de películas falló para {Query}.", model.Query);
            model.ErrorMessage = exception.Message;
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        try
        {
            var movie = await tmdbService.GetMovieDetailsAsync(id, cancellationToken);
            return movie is null ? NotFound() : View(movie);
        }
        catch (TmdbServiceException exception)
        {
            logger.LogWarning(exception, "No se pudieron cargar los detalles de la película {MovieId}.", id);
            return View("Error", new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier,
                Message = exception.Message
            });
        }
    }
}
