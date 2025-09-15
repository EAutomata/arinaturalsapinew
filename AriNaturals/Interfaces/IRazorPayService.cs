namespace AriNaturals.Interfaces
{
    public interface IRazorPayService
    {
        Task<string> CreateOrderAsync(decimal amount, string receiptId, string currency = "INR");
        bool VerifyPayment(string razorpayOrderId, string razorpayPaymentId, string razorpaySignature);
    }
}
