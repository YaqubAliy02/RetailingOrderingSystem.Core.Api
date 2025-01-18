namespace Infrastructure.Services.Payments
{
    public interface IPaymentService
    {
        Task<(string PaymentIntentId, string Message)> ProcessPaymentAsync(Guid orderId);
    }
}
