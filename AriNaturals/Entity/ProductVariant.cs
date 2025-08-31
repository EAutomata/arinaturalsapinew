using System;

namespace AriNaturals.Entity
{
    public class ProductVariant
    {
        public Guid VariantId { get; set; }
        public Guid ProductId { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Navigation
        public Product Product { get; set; }
    }
}
