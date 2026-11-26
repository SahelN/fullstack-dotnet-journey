using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Controllers;

[Route("api")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly MovieContext _context;

    public ReviewsController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/movies/{movieId}/reviews
    [HttpGet("movies/{movieId}/reviews")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForMovie(int movieId)
    {
        if (!await _context.Movies.AnyAsync(m => m.Id == movieId))
        {
            return NotFound($"Movie with id {movieId} does not exist.");
        }

        var reviews = await _context.Reviews
            .Where(r => r.MovieId == movieId)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                ReviewerName = r.ReviewerName,
                Comment = r.Comment,
                Rating = r.Rating
            })
            .ToListAsync();

        return Ok(reviews);
    }

    // POST: api/movies/{movieId}/reviews
    [HttpPost("movies/{movieId}/reviews")]
    public async Task<ActionResult<ReviewDto>> PostReview(int movieId, ReviewCreateDto dto)
    {
        if (!await _context.Movies.AnyAsync(m => m.Id == movieId))
        {
            return NotFound($"Movie with id {movieId} does not exist.");
        }

        var review = new Review
        {
            ReviewerName = dto.ReviewerName,
            Comment = dto.Comment,
            Rating = dto.Rating,
            MovieId = movieId
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var result = new ReviewDto
        {
            Id = review.Id,
            ReviewerName = review.ReviewerName,
            Comment = review.Comment,
            Rating = review.Rating
        };

        return CreatedAtAction(nameof(GetReviewsForMovie), new { movieId }, result);
    }

    // DELETE: api/reviews/{id}
    [HttpDelete("reviews/{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review is null)
        {
            return NotFound();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}