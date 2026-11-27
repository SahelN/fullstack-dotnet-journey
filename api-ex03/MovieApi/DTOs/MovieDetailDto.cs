namespace MovieApi.DTOs;

public record MovieDetailDto
{
    // Movie data
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public int? Year { get; init; }
    public int Duration { get; init; }
    public string? Genre { get; init; }

    // MovieDetails
    public string? Synopsis { get; init; }
    public string? Language { get; init; }
    public decimal? Budget { get; init; }

    // Related lists
    public List<ReviewDto> Reviews { get; init; } = [];
    public List<MovieActorDto> Actors { get; init; } = [];
}