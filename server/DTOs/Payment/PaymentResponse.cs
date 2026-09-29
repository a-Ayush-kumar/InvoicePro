using server.Entities;
namespace server.DTOs.Payment;

public class PaymentResponse
{
    public Guid PaymentId { get; set; }
    public Guid InvoiceId { get; set; }

    public decimal Amount { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }

    public string PayerName { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;

    public PaymentStatus Status { get; set; }

    public Guid RecordedByMerchantUserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}