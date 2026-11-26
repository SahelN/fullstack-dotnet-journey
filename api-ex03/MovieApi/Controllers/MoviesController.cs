using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models;
using MovieApi.Data;
using MovieApi.DTOs;

namespace MovieApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MovieContext _context;
    public MoviesController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/Movie
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(
        [FromQuery] string? genre,
        [FromQuery] int? year,
        [FromQuery] string? actor)
    {
        IQueryable<Movie> query = _context.Movies;

        if (!string.IsNullOrWhiteSpace(genre))
            query = query.Where(m => m.Genre != null && m.Genre.Name == genre);

        if (year is not null)
            query = query.Where(m => m.Year == year);

        if (!string.IsNullOrWhiteSpace(actor))
            query = query.Where(m => m.Actors.Any(a => a.Name == actor));

        var movies = await query
            .Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Year = m.Year,
                Duration = m.Duration,
                Genre = m.Genre != null ? m.Genre.Name : null
            })
            .ToListAsync();
        return Ok(movies);
    }

    // GET: api/Movie/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovie(int id)
    {
        var movie = await _context.Movies
            .Where(m => m.Id == id)
            .Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Year = m.Year,
                Duration = m.Duration,
                Genre = m.Genre != null ? m.Genre.Name : null
            })
            .FirstOrDefaultAsync();

        if (movie is null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    // PUT: api/Movie/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int id, MovieUpdateDto dto)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie is null)
        {
            return NotFound();
        }

        if (dto.GenreId is not null &&
            !await _context.Genres.AnyAsync(g => g.Id == dto.GenreId))
            return BadRequest($"Genre with id {dto.GenreId} does not exist.");

        movie.Title = dto.Title;
        movie.Year = dto.Year;
        movie.Duration = dto.Duration;
        movie.GenreId = dto.GenreId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/Movie
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto dto)
    {
        if (dto.GenreId is not null &&
            !await _context.Genres.AnyAsync(g => g.Id == dto.GenreId))
            return BadRequest($"Genre with id {dto.GenreId} does not exist.");

        var movie = new Movie
        {
            Title = dto.Title,
            Year = dto.Year,
            Duration = dto.Duration,
            GenreId = dto.GenreId
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        var result = await _context.Movies
            .Where(m => m.Id == movie.Id)
            .Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Year = m.Year,
                Duration = m.Duration,
                Genre = m.Genre != null ? m.Genre.Name : null
            })
            .FirstAsync();

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, result);
    }

    // DELETE: api/Movie/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie is null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
