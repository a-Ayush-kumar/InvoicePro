namespace server.DTOs.MerchantCustomer;

public class CustomerMerchantListItemResponse
{
    public Guid MerchantId { get; set; }

    public string LegalName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public bool MerchantIsActive { get; set; }
    public bool RelationshipIsActive { get; set; }

    public DateTime RelationshipCreatedAt { get; set; }
}