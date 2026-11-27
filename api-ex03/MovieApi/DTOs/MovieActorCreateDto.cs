using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public record MovieActorCreateDto
{
    [Required]
    public int? ActorId { get; init; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Role { get; init; } = string.Empty;
}