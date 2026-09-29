namespace server.Entities;

public class Invoice
{
    public Guid InvoiceId { get; set; }
    public Guid MerchantId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid CreatedByMerchantUserId { get; set; }
    public Guid? UpdatedByMerchantUserId { get; set; } //questionmark represent the optional value

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

    public Merchant Merchant { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public MerchantUser CreatedByMerchantUser { get; set; } = null!;
    public MerchantUser? UpdatedByMerchantUser { get; set; }

    public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
