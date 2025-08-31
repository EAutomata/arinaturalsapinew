using System;
using AriNaturals.Entity;

namespace AriNaturals.Entity
{
    public class ProductHighlight
    {
        public Guid HighlightId { get; set; }
        public Guid ProductId { get; set; }
        public string HighlightText { get; set; } = string.Empty;

        // Navigation
        public Product Product { get; set; }
    }
}
