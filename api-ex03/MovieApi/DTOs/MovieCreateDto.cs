using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public record MovieCreateDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; init; } = string.Empty;

    [Range(1888, 2100)]
    public int? Year { get; init; }

    [Range(1, 600)]
    public int Duration { get; init; }

    public int? GenreId { get; init; }
}