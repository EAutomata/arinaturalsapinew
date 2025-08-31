namespace AriNaturals.Models
{
    public class AddressRequest
    {
        public Guid AddressId { get; set; }
        public string AddressLine1 { get; set; } = default!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
    }
}
