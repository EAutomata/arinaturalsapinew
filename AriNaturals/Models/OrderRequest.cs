namespace AriNaturals.Models
{
    public class OrderRequest
    {        
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressRequest BillingAddress { get; set; }
        public AddressRequest ShippingAddress { get; set; }
        public List<OrderItemRequest> Items { get; set; }
        public bool UseBillingAsShipping { get; set; } = false;
    }
}
