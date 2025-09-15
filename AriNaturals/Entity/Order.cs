namespace AriNaturals.Entity
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string AddressType { get; set; } = string.Empty;
        public Guid BillingAddressId { get; set; }
        public Guid ShippingAddressId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string ShippingStatus { get; set; } = "Processing";
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OrderNumber { get; set; }
        public long OrderSequence { get; set; }
        public string RazorPayOrderId { get; set; }

        // Navigation
        public Customer Customer { get; set; }
        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
