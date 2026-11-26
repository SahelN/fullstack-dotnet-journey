namespace MovieApi.DTOs;

public record ReviewDto
{
    public int Id { get; init; }
    public string ReviewerName { get; init; } = string.Empty;
    public string Comment { get; init; } = string.Empty;
    public int Rating { get; init; }
}