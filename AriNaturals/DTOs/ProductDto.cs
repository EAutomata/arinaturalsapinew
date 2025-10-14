using AriNaturals.Entity;

namespace AriNaturals.DTOs
{
    public class ProductDto
    {
        public Guid ProductId { get; set; } = new Guid();
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public bool IsFeaturedProduct { get; set; }
        public ICollection<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
        public ICollection<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
        public ICollection<ProductHighlightDto> Highlights { get; set; } = new List<ProductHighlightDto>();
        public ICollection<ProductReviewDto> Reviews { get; set; } = new List<ProductReviewDto>();
    }

    public class ProductVariantDto
    {
        public Guid VariantId { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class ProductImageDto
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class ProductHighlightDto
    {
        public Guid HighlightId { get; set; }
        public string HighlightTitle { get; set; } = string.Empty;
        public string HighlightText { get; set; } = string.Empty;
        public List<ProductHighlightSectionDto> HighlightSections { get; set; } = new List<ProductHighlightSectionDto>();
    }

    public class ProductHighlightSectionDto
    {
        public string SectionTitle { get; set; } = string.Empty;
        public string SectionText { get; set; } = string.Empty;
        public List<string> SectionPoints { get; set; } = new();
    }

    public class ProductReviewDto
    {
        public Guid ReviewId { get; set; }
        public string? ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
