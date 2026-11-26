using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public record ActorCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    [Range(1850, 2026)]
    public int? BirthYear { get; init; }
}