using System;
using System.Collections.Generic;
using AriNaturals.Entity;

namespace AriNaturals.Entity
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public bool IsFeaturedProduct { get; set; }

        // Navigation
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductHighlight> Highlights { get; set; } = new List<ProductHighlight>();
        public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
