namespace AriNaturals.DTOs
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string RazorPayOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public PaymentDto Payment { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class CreateOrderRequest
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public decimal TotalAmount { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public AddressDto BillingAddress { get; set; }
        public bool IsBillingSameAsShipping { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class AddressDto
    {
        public string CustomerEmail { get; set; }
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    public class PaymentDto
    {
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }

    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public Guid VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
