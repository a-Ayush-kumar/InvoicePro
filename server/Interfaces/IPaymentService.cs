using server.DTOs.Payment;

namespace server.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponse?> CreateAsync(
        CreatePaymentRequest request);

    Task<PaymentResponse?> GetByIdAsync(
        Guid paymentId);
}