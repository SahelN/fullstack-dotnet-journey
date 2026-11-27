namespace MovieApi.DTOs;

public record MovieActorDto
{
    public int ActorId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}