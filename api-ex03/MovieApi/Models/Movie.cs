namespace MovieApi.Models;
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? Year { get; set; }      
    public int Duration { get; set; } // minuter
    public int? GenreId { get; set; } // FK
    public Genre? Genre { get; set; } // navigation
    public MovieDetails? MovieDetails { get; set; } // navigation
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<MovieActor> MovieActors { get; set; } = [];
}