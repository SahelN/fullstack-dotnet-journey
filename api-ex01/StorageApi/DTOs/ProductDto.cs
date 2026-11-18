namespace StorageApi.DTOs;

// Output till klienten – bara de fält som ska visas
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public int Count { get; set; }

    // Beräknat fält – lagras inte i databasen
    public int InventoryValue => Price * Count;
}
