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
    public DbSet<MovieActor> MovieActors { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MovieActor>(entity =>
        {
            // Composite key
            entity.HasKey(ma => new { ma.MovieId, ma.ActorId });

            // Movie 1:M MovieActor
            entity.HasOne(ma => ma.Movie)
                  .WithMany(m => m.MovieActors)
                  .HasForeignKey(ma => ma.MovieId);

            // Actor 1:M MovieActor
            entity.HasOne(ma => ma.Actor)
                  .WithMany(a => a.MovieActors)
                  .HasForeignKey(ma => ma.ActorId);

            entity.Property(ma => ma.Role)
                  .HasMaxLength(100);
        });
    }
}