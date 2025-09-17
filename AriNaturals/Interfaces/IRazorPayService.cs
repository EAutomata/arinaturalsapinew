using Razorpay.Api;

namespace AriNaturals.Interfaces
{
    public interface IRazorPayService
    {
        Task<string> CreateOrderAsync(decimal amount, string receiptId, string currency = "INR");
        Payment VerifyPayment(string razorpayOrderId, string razorpayPaymentId, string razorpaySignature);
    }
}
