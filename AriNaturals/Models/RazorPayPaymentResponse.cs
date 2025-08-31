namespace AriNaturals.Models
{
    public class RazorpayPaymentResponse
    {
        public string RazorpayPaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
        public string RazorpaySignature { get; set; }
        public string Email { get; set; }
    }
}
