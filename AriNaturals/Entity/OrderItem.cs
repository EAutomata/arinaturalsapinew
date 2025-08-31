using System;

namespace AriNaturals.Entity
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // Navigation
        public Order Order { get; set; }
        public Product Product { get; set; }
        public ProductVariant Variant { get; set; }
    }
}
