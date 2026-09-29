namespace server.DTOs.MerchantCustomer;

public class MerchantCustomerResponse
{
    public Guid MerchantId { get; set; }
    public Guid CustomerId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}