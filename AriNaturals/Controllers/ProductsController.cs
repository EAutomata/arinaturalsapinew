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
            return BadRequest("Product ID mismatch");

        var product = await _context.Products
            .Include(p => p.Variants)
            .Include(p => p.Images)
            .Include(p => p.Highlights)
                .ThenInclude(h => h.HighlightSections)
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product == null)
            return NotFound();

        _mapper.Map(productDto, product);

        var variantIds = productDto.Variants.Select(v => v.VariantId).ToList();
        product.Variants.ToList().RemoveAll(v => !variantIds.Contains(v.VariantId));

        foreach (var variantDto in productDto.Variants)
        {
            var existingVariant = product.Variants.FirstOrDefault(v => v.VariantId == variantDto.VariantId);
            if (existingVariant != null)
            {
                _mapper.Map(variantDto, existingVariant);
            }
            else
            {
                var newVariant = _mapper.Map<ProductVariant>(variantDto);
                product.Variants.Add(newVariant);
            }
        }

        var imageIds = productDto.Images.Select(i => i.ImageId).ToList();
        product.Images.ToList().RemoveAll(i => !imageIds.Contains(i.ImageId));

        foreach (var imageDto in productDto.Images)
        {
            var existingImage = product.Images.FirstOrDefault(i => i.ImageId == imageDto.ImageId);
            if (existingImage != null)
            {
                _mapper.Map(imageDto, existingImage);
            }
            else
            {
                var newImage = _mapper.Map<ProductImage>(imageDto);
                product.Images.Add(newImage);
            }
        }

        var highlightIds = productDto.Highlights.Select(h => h.HighlightId).ToList();
        product.Highlights.ToList().RemoveAll(h => !highlightIds.Contains(h.HighlightId));

        foreach (var highlightDto in productDto.Highlights)
        {
            var existingHighlight = product.Highlights.FirstOrDefault(h => h.HighlightId == highlightDto.HighlightId);
            if (existingHighlight != null)
            {
                _mapper.Map(highlightDto, existingHighlight);

                var sectionIds = highlightDto.HighlightSections.Select(s => s.SectionId).ToList();
                existingHighlight.HighlightSections.ToList().RemoveAll(s => !sectionIds.Contains(s.SectionId));

                foreach (var sectionDto in highlightDto.HighlightSections)
                {
                    var existingSection = existingHighlight.HighlightSections.FirstOrDefault(s => s.SectionId == sectionDto.SectionId);
                    if (existingSection != null)
                        _mapper.Map(sectionDto, existingSection);
                    else
                        existingHighlight.HighlightSections.Add(_mapper.Map<ProductHighlightSection>(sectionDto));
                }
            }
            else
            {
                var newHighlight = _mapper.Map<ProductHighlight>(highlightDto);
                product.Highlights.Add(newHighlight);
            }
        }

        await _context.SaveChangesAsync();

        return Ok();
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
