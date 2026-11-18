namespace StorageApi.DTOs;

// Svar från GET /api/products/stats
public class ProductStatsDto
{
    public int TotalProducts { get; set; }        // antal produkter (rader)
    public int TotalItemsInStock { get; set; }    // summan av Count
    public int TotalInventoryValue { get; set; }  // summan av Price * Count
    public double AveragePrice { get; set; }      // snittpris per produkt
}
