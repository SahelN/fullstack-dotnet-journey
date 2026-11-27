using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Controllers;

[Route("api")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly MovieContext _context;

    public ActorsController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/actors
    [HttpGet("actors")]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
    {
        var actors = await _context.Actors
            .Select(a => new ActorDto
            {
                Id = a.Id,
                Name = a.Name,
                BirthYear = a.BirthYear
            })
            .ToListAsync();

        return Ok(actors);
    }

    // GET: api/actors/{id}
    [HttpGet("actors/{id}")]
    public async Task<ActionResult<ActorDto>> GetActor(int id)
    {
        var actor = await _context.Actors
            .Where(a => a.Id == id)
            .Select(a => new ActorDto
            {
                Id = a.Id,
                Name = a.Name,
                BirthYear = a.BirthYear
            })
            .FirstOrDefaultAsync();

        if (actor is null)
        {
            return NotFound();
        }

        return Ok(actor);
    }

    // POST: api/actors
    [HttpPost("actors")]
    public async Task<ActionResult<ActorDto>> PostActor(ActorCreateDto dto)
    {
        var actor = new Actor
        {
            Name = dto.Name,
            BirthYear = dto.BirthYear
        };

        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        var result = new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name,
            BirthYear = actor.BirthYear
        };

        return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, result);
    }

    // PUT: api/actors/{id}
    [HttpPut("actors/{id}")]
    public async Task<IActionResult> PutActor(int id, ActorUpdateDto dto)
    {
        var actor = await _context.Actors.FindAsync(id);

        if (actor is null)
        {
            return NotFound();
        }

        actor.Name = dto.Name;
        actor.BirthYear = dto.BirthYear;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/movies/{movieId}/actors/{actorId}
    [HttpPost("movies/{movieId}/actors/{actorId}")]
    public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
    {
        var movie = await _context.Movies
            .Include(m => m.MovieActors)
            .FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie is null)
        {
            return NotFound($"Movie with id {movieId} does not exist.");
        }

        var actor = await _context.Actors.FindAsync(actorId);

        if (actor is null)
        {
            return NotFound($"Actor with id {actorId} does not exist.");
        }

        if (movie.MovieActors.Any(ma => ma.ActorId == actorId))
        {
            return Conflict($"Actor {actorId} is already in movie {movieId}.");
        }

        movie.MovieActors.Add(new MovieActor { ActorId = actorId, Role = string.Empty });
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/movies/{movieId}/actors
    [HttpPost("movies/{movieId}/actors")]
    public async Task<ActionResult<MovieActorDto>> AddActorToMovieWithRole(int movieId, MovieActorCreateDto dto)
    {
        if (!await _context.Movies.AnyAsync(m => m.Id == movieId))
        {
            return NotFound($"Movie with id {movieId} does not exist.");
        }

        var actor = await _context.Actors.FindAsync(dto.ActorId!.Value);

        if (actor is null)
        {
            return NotFound($"Actor with id {dto.ActorId} does not exist.");
        }

        if (await _context.MovieActors.AnyAsync(ma => ma.MovieId == movieId && ma.ActorId == dto.ActorId))
        {
            return Conflict($"Actor {dto.ActorId} is already in movie {movieId}.");
        }

        var movieActor = new MovieActor
        {
            MovieId = movieId,
            ActorId = actor.Id,
            Role = dto.Role
        };

        _context.MovieActors.Add(movieActor);
        await _context.SaveChangesAsync();

        var result = new MovieActorDto
        {
            ActorId = actor.Id,
            Name = actor.Name,
            Role = movieActor.Role
        };

        return Created($"/api/movies/{movieId}/details", result);
    }
}