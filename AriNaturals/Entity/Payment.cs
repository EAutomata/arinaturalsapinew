using System;

namespace AriNaturals.Entity
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }

        // Navigation
        public Order Order { get; set; }
    }
}
