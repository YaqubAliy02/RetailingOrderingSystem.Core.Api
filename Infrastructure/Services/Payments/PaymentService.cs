using Application.Abstraction;
using Application.Helpers;
using Application.Repositories;
using Domain.Models;
using Microsoft.Extensions.Options;
using Stripe;

namespace Infrastructure.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentSettings _paymentSettings;
        private readonly IRetailingOrderingSystemDbContext context;

        public PaymentService(
            IOptions<PaymentSettings> paymentSettings,
            IRetailingOrderingSystemDbContext context)
        {
            _paymentSettings = paymentSettings.Value;
            StripeConfiguration.ApiKey = _paymentSettings.SecretKey;
            this.context = context;
        }

        public async Task<(string PaymentIntentId, string Message)> ProcessPaymentAsync(Guid orderId)
        {
            var order = await this.context.Orders.FindAsync(orderId);
            if (order == null) throw new KeyNotFoundException("Order not found.");

            if (order.PaymentStatus == "Paid")
            {
                throw new InvalidOperationException("Order is already paid.");
            }

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = (long)(order.TotalAmount * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
            });

            order.PaymentStatus = "Paid";
            order.PaymentIntent = paymentIntent.Id;
            this.context.Orders.Update(order);
            await this.context.SaveChangesAsync();

            return (paymentIntent.Id, "Payment processed successfully.");
        }

    }
}
