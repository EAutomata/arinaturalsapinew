using System.Net.Mail;
using System.Net;
using AriNaturals.Interfaces;

namespace AriNaturals.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var fromAddress = new MailAddress("mail@arinaturals.com", "Arinaturals");
            var toAddress = new MailAddress("engautomata@gmail.com");
            string fromPassword = "Arinatural@!@#";
            subject = "Your Order Confirmation";

            var htmlBody = GenerateOrderEmail(
                                    "AriNaturals",
                                    "http://localhost:5173/public/ekart.jpeg",
                                     "Prabhu Prakash",
                                    new List<(string, string, int, decimal, string)>
                                    {
                                        ("FlaxSeeds", "250g", 1, 120.00m, "http://localhost:5173/public/ari-naturals-sunflower-seeds.jpeg"),
                                        ("Gingelly Oil (Sesame Oil)", "500ml", 1, 320.00m, "http://localhost:5173/src/assets/ari-naturals-coconut-oil.jpeg")
                                    },
                                    "Prabhu Prakash\n123 Main Street\nChennai, TN 600037\nIndia"
                                );

            using (var smtp = new SmtpClient
            {
                Host = "smtpout.secureserver.net",
                Port = 587, // Try 587 first
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000 // 20 seconds
            })
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                From = new MailAddress(fromAddress.Address, "AriNaturals.com"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

        public static string GenerateOrderEmail(
                                            string siteName,
                                            string logoUrl,
                                            string customerName,
                                            List<(string ItemName, string Measurement, int Quantity, decimal UnitPrice, string ImageUrl)> items,
                                            string shippingAddress)
        {
            // Calculate total dynamically
            decimal totalValue = items.Sum(i => i.UnitPrice * i.Quantity);

            var itemsHtml = string.Join("", items.Select(i => $@"
                                <tr>
                                    <td style='padding:8px;border:1px solid #ddd;'>
                                        <div style='display:flex; align-items:center;'>
                                            <div>
                                                <strong>{i.ItemName}</strong><br>
                                                <small>{i.Measurement}</small>
                                            </div>
                                        </div>
                                    </td>
                                    <td style='padding:8px;border:1px solid #ddd;text-align:right;'>{i.UnitPrice:C}</td>
                                    <td style='padding:8px;border:1px solid #ddd;text-align:center;'>{i.Quantity}</td>
                                    <td style='padding:8px;border:1px solid #ddd;text-align:right;'>{(i.UnitPrice * i.Quantity):C}</td>
                                </tr>
                            "));

            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Order Confirmation</title>
                </head>
                <body style='font-family: Arial, sans-serif; background-color: #f8f8f8; margin:0; padding:0;'>
        
                    <!-- Header with logo -->
                    <div style='background-color:#232f3e; padding:10px; text-align:center; display:flex; align-items:center; justify-content:center;'>
                        <img src='{logoUrl}' alt='{siteName}' style='height:50px; margin-right:10px;'>
                        <span style='color:white; font-size:24px; font-weight:bold;'>{siteName}</span>
                    </div>

                    <div style='max-width:600px; margin:20px auto; background:#fff; padding:20px; border-radius:8px;'>
                        <h2 style='color:#333;'>Thank you for your order, {customerName}!</h2>
                        <p>We’ve received your payment and your order is now being processed.</p>

                        <h3 style='margin-top:30px;'>Order Details</h3>
                        <table style='width:100%; border-collapse:collapse;'>
                            <thead>
                                <tr style='background:#f0f0f0;'>
                                    <th style='padding:8px;border:1px solid #ddd;text-align:left;'>Item</th>
                                    <th style='padding:8px;border:1px solid #ddd;text-align:right;'>Unit Price</th>
                                    <th style='padding:8px;border:1px solid #ddd;text-align:center;'>Qty</th>
                                    <th style='padding:8px;border:1px solid #ddd;text-align:right;'>Subtotal</th>
                                </tr>
                            </thead>
                            <tbody>
                                {itemsHtml}
                                <tr>
                                    <td colspan='3' style='padding:8px;border:1px solid #ddd;text-align:right; font-weight:bold;'>Total</td>
                                    <td style='padding:8px;border:1px solid #ddd;text-align:right; font-weight:bold;'>{totalValue:C}</td>
                                </tr>
                            </tbody>
                        </table>

                        <h3 style='margin-top:30px;'>Shipping Address</h3>
                        <p style='white-space:pre-line; background:#f9f9f9; padding:10px; border:1px solid #ddd; border-radius:4px;'>{shippingAddress}</p>

                        <p style='margin-top:40px;'>If you have any questions, reply to this email or contact our support team.</p>
                        <p style='color:#999;font-size:12px;'>This is an automated email. Please do not reply directly to this message.</p>
                    </div>
                </body>
                </html>";
        }

    }
}
