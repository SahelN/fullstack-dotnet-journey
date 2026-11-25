using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data;
public class MovieContext(DbContextOptions<MovieContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; } = default!;
}