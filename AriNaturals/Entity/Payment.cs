namespace AriNaturals.Entity
{
    public class Payment
    {
        public Guid PaymentId { get; set; } = Guid.NewGuid();
        public string RazorpayPaymentId { get; set; } = string.Empty;
        public string RazorpayOrderId { get; set; } = string.Empty;
        public string? RazorpaySignature { get; set; }
        public string Status { get; set; } = "created";
        public string? Method { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "INR";
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }

}
