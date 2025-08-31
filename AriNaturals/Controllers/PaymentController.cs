using AriNaturals.Interfaces;
using AriNaturals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using Razorpay.Api.Errors;

namespace AriNaturals.Controllers
{
    [AllowAnonymous]
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public PaymentController(IConfiguration config, IEmailService emailService)
        {
            _config = config;
            _emailService = emailService;
        }

        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] RazorpayOrderRequest request)
        {
            var key = _config["Razorpay:Key"];
            var secret = _config["Razorpay:Secret"];

            RazorpayClient client = new RazorpayClient(key, secret);

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", 50000);
            options.Add("receipt", "order_rcptid_11");
            options.Add("currency", "INR");

            Order order = client.Order.Create(options);

            // Save order info to DB if needed here

            return Ok(new
            {
                orderId = order["id"].ToString(),
                amount = order["amount"],
                currency = order["currency"],
                key = key
            });
        }

        [HttpPost("verify-payment")]
        public async Task<IActionResult> VerifyPayment([FromBody] RazorpayPaymentResponse response)
        {
            bool paymentFailed = false;
            var key = _config["Razorpay:Key"];
            var secret = _config["Razorpay:Secret"];
            RazorpayClient client = new RazorpayClient(key, secret);

            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("razorpay_order_id", "order_IEIaMR65");
            options.Add("razorpay_payment_id", "pay_IH4NVgf4Dreq1l");
            options.Add("razorpay_signature", "0d4e745a1838664ad6c9c9902212a32d627d68e917290b0ad5f08ff4561bc50");

            try
            {
                Utils.verifyPaymentSignature(options);
            }
            catch(SignatureVerificationError ex)
            {
                paymentFailed = true;
                await _emailService.SendEmailAsync(response.Email, "Payment Failed", "Payment verification failed.");
            }

            if (paymentFailed)
            {
                return BadRequest(new { status = "failed" });
            }
            else
            {
                return Ok();
            }
           
        }

        [HttpPost("Send-email")]
        public async Task<IActionResult> SendEmail([FromBody] string toemail)
        {
            await _emailService.SendEmailAsync("engautomata@gmail.com", "Payment success", "Payment verification success.");
            return Ok();
        }
    }
}
