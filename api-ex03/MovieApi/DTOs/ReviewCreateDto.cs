using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public record ReviewCreateDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string ReviewerName { get; init; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Comment { get; init; } = string.Empty;

    [Range(1,5)]
    public int Rating { get; init; }
}

