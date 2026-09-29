using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.Payment;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class PaymentService : IPaymentService
{
    private readonly InvoiceDbContext _context;

    public PaymentService(InvoiceDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentResponse?> CreateAsync(
        CreatePaymentRequest request)
    {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.InvoiceId == request.InvoiceId);

        if (invoice == null)
            return null;

        var recordedByMerchantUser = await _context.MerchantUsers
            .FirstOrDefaultAsync(mu =>
                mu.MerchantUserId == request.RecordedByMerchantUserId &&
                mu.MerchantId == invoice.MerchantId &&
                mu.IsActive);

        if (recordedByMerchantUser == null)
            return null;

        var payment = new Payment
        {
            InvoiceId = request.InvoiceId,

            Amount = request.Amount,
            TransactionId = request.TransactionId,
            TransactionDate = request.TransactionDate,

            PayerName = request.PayerName,
            BankName = request.BankName,
            PaymentMethod = request.PaymentMethod,

            Status = request.Status,

            RecordedByMerchantUserId =
                request.RecordedByMerchantUserId,

            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);

        await _context.SaveChangesAsync();

        return MapToResponse(payment);
    }

    public async Task<PaymentResponse?> GetByIdAsync(
        Guid paymentId)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

        if (payment == null)
            return null;

        return MapToResponse(payment);
    }

    private static PaymentResponse MapToResponse(
        Payment payment)
    {
        return new PaymentResponse
        {
            PaymentId = payment.PaymentId,
            InvoiceId = payment.InvoiceId,

            Amount = payment.Amount,
            TransactionId = payment.TransactionId,
            TransactionDate = payment.TransactionDate,

            PayerName = payment.PayerName,
            BankName = payment.BankName,
            PaymentMethod = payment.PaymentMethod,

            Status = payment.Status,

            RecordedByMerchantUserId =
                payment.RecordedByMerchantUserId,

            CreatedAt = payment.CreatedAt,
            UpdatedAt = payment.UpdatedAt
        };
    }
}