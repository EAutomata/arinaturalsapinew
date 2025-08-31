using System;
using AriNaturals.Entity;

namespace AriNaturals.Entity
{
    public class ProductImage
    {
        public Guid ImageId { get; set; }
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // Navigation
        public Product Product { get; set; }
    }
}
