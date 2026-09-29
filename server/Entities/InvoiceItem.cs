namespace server.Entities;

public class InvoiceItem
{
    public Guid InvoiceItemId { get; set; }
    public Guid InvoiceId { get; set; }
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

    public Invoice Invoice { get; set; } = null!;
    public Service Service { get; set; } = null!;
}