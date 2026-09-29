namespace server.DTOs.MerchantUser;

public class MerchantUserResponse
{
    public Guid MerchantUserId { get; set; }
    public Guid UserId { get; set; }
    public Guid MerchantId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}