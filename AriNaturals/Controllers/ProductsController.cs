using System.Text.Json;
using AriNaturals.DataAccess;
using AriNaturals.DTOs;
using AriNaturals.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ProductDto>> GetProducts()
    {
        var products = await _context.Products
            .Include(p => p.Variants)
            .Include(p => p.Images)
            .Include(p => p.Highlights)
            .ThenInclude(p => p.HighlightSections)
            .ToListAsync();

        if (!products.Any())
            return NotFound();

        var productsList = _mapper.Map<List<ProductDto>>(products);
        return Ok(productsList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
    {
        var product = await _context.Products
            .Include(p => p.Variants)
            .Include(p => p.Images)
            .Include(p => p.Highlights)
            .ThenInclude(p => p.HighlightSections)
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product == null)
            return NotFound();

        return _mapper.Map<ProductDto>(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        product.ProductId = Guid.NewGuid();

        if (product.Variants != null)
        {
            foreach (var variant in product.Variants)
                variant.ProductId = product.ProductId;
        }
        else
        {
            return BadRequest("Variants cannot be empty");
        }

        if (product.Images != null)
        {
            foreach (var image in product.Images)
                image.ProductId = product.ProductId;
        }
        else
        {
            return BadRequest("Images cannot be empty");
        }

        if (product.Highlights != null)
        {
            foreach (var highlight in product.Highlights)
            {
                highlight.ProductId = product.ProductId;
                if (highlight.HighlightSections != null)
                {
                    foreach (var section in highlight.HighlightSections)
                    {
                        section.PointsJson = JsonSerializer.Serialize(section.SectionPoints);
                    }
                }
            }
        }
        else
        {
            return BadRequest("Highlights cannot be empty");
        }

        // Add and save
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var createdProduct = _mapper.Map<ProductDto>(product);

        return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, ProductDto productDto)
    {
        if (id != productDto.ProductId)
            return BadRequest();

        var product = await _context.Products
            .Include(p => p.Variants)
            .Include(p => p.Images)
            .Include(p => p.Highlights)
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product == null)
            return NotFound();

        _mapper.Map(productDto, product);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
