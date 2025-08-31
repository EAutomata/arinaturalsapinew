namespace AriNaturals.Models
{
    public class ProductRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public bool IsFeaturedProduct { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ProductVariantRequest> Variants { get; set; } = new List<ProductVariantRequest>();
        public List<string> Images { get; set; } = new List<string>();
        public List<string> Highlights { get; set; } = new List<string>();
    }

    public class ProductVariantRequest
    {
        public string VariantName { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

}
