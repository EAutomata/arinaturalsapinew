using AriNaturals.DataAccess;
using AriNaturals.Entity;
using AriNaturals.Models;
using Microsoft.AspNetCore.Mvc;

namespace AriNaturals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                OrderId = request.OrderId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                PaidAt = DateTime.UtcNow,
                Status = "Success" // Example: integrate with Razorpay/Stripe
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return Ok(payment.PaymentId);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();
            return payment;
        }
    }

}
