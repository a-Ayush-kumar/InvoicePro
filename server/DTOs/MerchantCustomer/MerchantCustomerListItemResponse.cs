namespace server.DTOs.MerchantCustomer;

public class MerchantCustomerListItemResponse
{
    public Guid CustomerId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Gstin { get; set; }

    public bool CustomerIsActive { get; set; }
    public bool RelationshipIsActive { get; set; }

    public DateTime RelationshipCreatedAt { get; set; }
}