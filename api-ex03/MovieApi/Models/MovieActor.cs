namespace MovieApi.Models;

public class MovieActor
{
    public int MovieId { get; set; }                 // FK
    public Movie Movie { get; set; } = null!;        // navigation

    public int ActorId { get; set; }                 // FK
    public Actor Actor { get; set; } = null!;        // navigation

    public string Role { get; set; } = string.Empty;
}