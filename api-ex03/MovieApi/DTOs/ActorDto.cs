namespace MovieApi.DTOs;

public record ActorDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int? BirthYear { get; init; }
}