namespace server.Entities;

public class Service
{
    public Guid ServiceId { get; set; }
    public Guid MerchantId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string HsnSacCode { get; set; } = string.Empty;
    public string DefaultUnit { get; set; } = string.Empty;

    public decimal DefaultUnitPrice {get; set;}
    public decimal DefaultTaxRate {get; set;}

    public bool IsActive {get; set;}
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Merchant Merchant { get; set; } = null!;
}
