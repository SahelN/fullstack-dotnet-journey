using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Data;
using StorageApi.DTOs;
using StorageApi.Models;

namespace StorageApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly StorageContext _context;

    public ProductsController(StorageContext context)
    {
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await _context.Products
            .OrderBy(p => p.Id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Count = p.Count
            })
            .ToListAsync();

        return Ok(products);
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(ToDto(product));
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, CreateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        // Uppdatera bara de fält som får ändras – Id rörs inte
        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Category = dto.Category;
        product.Shelf = dto.Shelf;
        product.Count = dto.Count;
        product.Description = dto.Description;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto dto)
    {
        // [ApiController] har redan validerat dto:n – vid fel returneras 400 automatiskt
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Category = dto.Category,
            Shelf = dto.Shelf,
            Count = dto.Count,
            Description = dto.Description
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, ToDto(product));
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Mappar entitet -> DTO
    private static ProductDto ToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        Count = product.Count
    };
}
