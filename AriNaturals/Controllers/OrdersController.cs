using System.Net;
using AriNaturals.DataAccess;
using AriNaturals.DTOs;
using AriNaturals.Entity;
using AriNaturals.Interfaces;
using AriNaturals.Utilities;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IRazorPayService _razorPayService;

    public OrdersController(AppDbContext context, IMapper mapper, IRazorPayService razorPayService)
    {
        _context = context;
        _mapper = mapper;
        _razorPayService = razorPayService;
    }

    [HttpGet("{orderNo}")]
    public async Task<ActionResult<OrderDto>> GetOrder(string orderNo)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Payments)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNo);

        if (order == null)
            return NotFound();

        return _mapper.Map<OrderDto>(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var customer = await _context.Customers
                        .FirstOrDefaultAsync(c => c.Email == request.CustomerEmail);

            if (customer == null)
            {
                customer = new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    FullName = request.CustomerName,
                    Email = request.CustomerEmail,
                    Phone = request.CustomerPhone
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            Address shippingAddress = null, billingAddress = null;

            if (request.ShippingAddress != null)
            {
                shippingAddress = _mapper.Map<Address>(request.ShippingAddress);
                shippingAddress.CustomerId = customer.CustomerId;
                _context.Addresses.Add(shippingAddress);
            }

            if (request.IsBillingSameAsShipping)
            {
                billingAddress = shippingAddress;
            }
            else if (request.BillingAddress != null)
            {
                billingAddress = _mapper.Map<Address>(request.BillingAddress);
                billingAddress.CustomerId = customer.CustomerId;
                _context.Addresses.Add(billingAddress);
            }

            await _context.SaveChangesAsync();

            var lastOrder = await _context.Orders.OrderByDescending(o => o.OrderSequence).FirstOrDefaultAsync();
            var nextId = (lastOrder?.OrderSequence ?? 0) + 1;

            var order = new Order
            {
                CustomerId = customer.CustomerId,
                OrderNumber = Tools.GenerateOrderNumber(nextId),
                BillingAddressId = (Guid)(billingAddress?.AddressId ?? shippingAddress?.AddressId),
                ShippingAddressId = (Guid)(shippingAddress?.AddressId ?? billingAddress?.AddressId),
                AddressType = request.IsBillingSameAsShipping ? "Both" : "Separate",
                Status = "Pending",
                ShippingStatus = "Processing",
                CreatedAt = DateTime.UtcNow,
                OrderSequence = nextId,
                TotalAmount = request.Items.Sum(oi => oi.Quantity * oi.UnitPrice),
                RazorPayOrderId = string.Empty
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var razorpayOrderId = await _razorPayService.CreateOrderAsync(order.TotalAmount, order.OrderNumber);

            // 3. Update order with Razorpay Order Id
            order.RazorPayOrderId = razorpayOrderId;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            // 4. Add OrderItems linked to the OrderId
            foreach (var item in request.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    VariantId = item.VariantId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice,
                };

                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var orderDto = _mapper.Map<OrderDto>(order);

            return CreatedAtAction(nameof(CreateOrder), new { id = order.RazorPayOrderId });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            throw new Exception("Order creation failed due to Razorpay error", ex);
        }
    }

}
