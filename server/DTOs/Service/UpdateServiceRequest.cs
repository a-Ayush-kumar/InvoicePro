namespace server.DTOs.Service;

public class UpdateServiceRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string HsnSacCode { get; set; } = string.Empty;
    public string DefaultUnit { get; set; } = string.Empty;
    public decimal DefaultUnitPrice { get; set; }
    public decimal DefaultTaxRate { get; set; }
}