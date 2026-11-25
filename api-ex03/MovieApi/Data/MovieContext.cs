using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data;
public class MovieContext(DbContextOptions<MovieContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<Genre> Genres { get; set; } = default!;
    public DbSet<MovieDetails> MovieDetails { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<Actor> Actors { get; set; } = default!;
}