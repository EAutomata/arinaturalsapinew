using AriNaturals.DataAccess;
using AriNaturals.Entity;
using AriNaturals.Interfaces;
using AriNaturals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AriNaturals.Controllers
{
    [AllowAnonymous]
    public class PaymentsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly IRazorPayService _razorPayService;
        private readonly AppDbContext _context;

        public PaymentsController(IConfiguration config, IEmailService emailService, IRazorPayService razorPayService, AppDbContext context)
        {
            _config = config;
            _emailService = emailService;
            _razorPayService = razorPayService;
            _context = context;
        }

        [HttpPost("verify-payment")]
        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentRequest request)
        {
            var razorPayPaymentInfo = _razorPayService.VerifyPayment(request.RazorpayOrderId, request.RazorpayPaymentId, request.RazorpaySignature);
            if (razorPayPaymentInfo == null)
                return BadRequest("Invalid signature. Payment verification failed.");

            var payment = new Payment
            {
                RazorpayPaymentId = request.RazorpayPaymentId,
                RazorpayOrderId = razorPayPaymentInfo["order_id"],
                RazorpaySignature = request.RazorpaySignature,
                Status = razorPayPaymentInfo["status"],
                Method = razorPayPaymentInfo["method"],
                Amount = razorPayPaymentInfo["amount"] / 100M,
                Currency = razorPayPaymentInfo["currency"],
                Email = razorPayPaymentInfo["email"],
                Contact = razorPayPaymentInfo["contact"],
                OrderId = request.OrderId
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Ok(payment);
        }

        [HttpPost("Send-email")]
        public async Task<IActionResult> SendEmail([FromBody] string toemail)
        {
            await _emailService.SendEmailAsync("engautomata@gmail.com", "Payment success", "Payment verification success.");
            return Ok();
        }
    }
}
