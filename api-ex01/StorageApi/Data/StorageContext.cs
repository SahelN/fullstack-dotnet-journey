using Microsoft.EntityFrameworkCore;
using StorageApi.Models;

namespace StorageApi.Data;

public class StorageContext(DbContextOptions<StorageContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = default!;
}
