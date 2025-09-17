using AriNaturals.Interfaces;
using AriNaturals.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Razorpay.Api;
using Razorpay.Api.Errors;

namespace AriNaturals.Services
{
    public class RazorPayService : IRazorPayService
    {
        private readonly string _key;
        private readonly string _secret;
        private readonly ILogger<RazorPayService> _logger;

        public RazorPayService(IConfiguration config, ILogger<RazorPayService> logger)
        {
            _key = config["Razorpay:Key"];
            _secret = config["Razorpay:Secret"];
            _logger = logger;
        }

        /// <summary>
        /// Creates an order in Razorpay
        /// </summary>
        public async Task<string> CreateOrderAsync(decimal amount, string receiptId, string currency = "INR")
        {
            try
            {
                var client = new RazorpayClient(_key, _secret);

                // amount in paise
                var options = new Dictionary<string, object>
                {
                    { "amount", (int)(amount * 100) },
                    { "currency", currency },
                    { "receipt", receiptId },
                    { "payment_capture", 1 }
                };

                Order order = client.Order.Create(options);
                return order["id"].ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Razorpay order for receipt {ReceiptId}", receiptId);
                throw;
            }
        }

        /// <summary>
        /// Verifies the payment signature returned by Razorpay after checkout
        /// </summary>
        public Payment VerifyPayment(string razorpayOrderId, string razorpayPaymentId, string razorpaySignature)
        {
            try
            {
                var client = new RazorpayClient(_key, _secret);
                var attributes = new Dictionary<string, string>
                {
                    { "razorpay_order_id", razorpayOrderId },
                    { "razorpay_payment_id", razorpayPaymentId },
                    { "razorpay_signature", razorpaySignature }
                };

                Utils.verifyPaymentSignature(attributes);

                Payment payment = client.Payment.Fetch(razorpayPaymentId);
                string status = payment["status"];
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Razorpay signature verification failed for order {OrderId}", razorpayOrderId);
                return null;
            }
        }
    }
}
