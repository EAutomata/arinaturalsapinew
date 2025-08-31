using System;

namespace AriNaturals.Entity
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public Guid CustomerId { get; set; }
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Customer Customer { get; set; }
    }
}
