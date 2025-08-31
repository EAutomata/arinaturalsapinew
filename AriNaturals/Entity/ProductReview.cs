using System;

namespace AriNaturals.Entity
{
    public class ProductReview
    {
        public Guid ReviewId { get; set; }
        public Guid ProductId { get; set; }
        public string? ReviewerName { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Product Product { get; set; }
    }
}
