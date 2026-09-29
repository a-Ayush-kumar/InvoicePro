using server.Entities;

namespace server.DTOs.Invoice;

public class InvoiceDetailResponse
{
    public Guid InvoiceId { get; set; }
    public Guid MerchantId { get; set; }

    public InvoiceCustomerResponse Customer { get; set; } = null!;

    public Guid CreatedByMerchantUserId { get; set; }
    public Guid? UpdatedByMerchantUserId { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }

    public string PlaceOfSupply { get; set; } = string.Empty;
    public bool ReverseCharge { get; set; }

    public string Currency { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal TotalAmount { get; set; }

    public InvoiceStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string TermsAndCondition { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<InvoiceDetailItemResponse> InvoiceItems { get; set; } = new();
    public List<InvoiceDetailPaymentResponse> Payments { get; set; } = new();
}

public class InvoiceCustomerResponse
{
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Gstin { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class InvoiceDetailItemResponse
{
    public Guid InvoiceItemId { get; set; }
    public Guid ServiceId { get; set; }

    public string Description { get; set; } = string.Empty;
    public string HsnSacCode { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal TaxRate { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal LineTotal { get; set; }

    public string ServiceName { get; set; } = string.Empty;
}

public class InvoiceDetailPaymentResponse
{
    public Guid PaymentId { get; set; }
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