using System.ComponentModel.DataAnnotations;

namespace StorageApi.DTOs;

// Input vid POST och PUT – samma fält som Product, men utan Id (Id sätts av databasen)
public class CreateProductDto
{
    [Required(ErrorMessage = "Namn är obligatoriskt.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Namn måste vara 2–100 tecken.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, 1_000_000, ErrorMessage = "Pris måste vara mellan 0 och 1 000 000.")]
    public int Price { get; set; }

    [Required(ErrorMessage = "Kategori är obligatorisk.")]
    [StringLength(50, ErrorMessage = "Kategori får vara max 50 tecken.")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hylla är obligatorisk.")]
    [StringLength(20, ErrorMessage = "Hylla får vara max 20 tecken.")]
    public string Shelf { get; set; } = string.Empty;

    [Range(0, 100_000, ErrorMessage = "Antal måste vara mellan 0 och 100 000.")]
    public int Count { get; set; }

    [StringLength(500, ErrorMessage = "Beskrivning får vara max 500 tecken.")]
    public string? Description { get; set; }
}
