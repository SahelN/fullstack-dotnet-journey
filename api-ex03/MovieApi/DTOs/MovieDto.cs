namespace MovieApi.DTOs;

public record MovieDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public int? Year { get; init; }
    public int Duration { get; init; }
    public string? Genre { get; init; }
}