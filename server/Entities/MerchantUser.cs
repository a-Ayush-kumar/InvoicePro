namespace server.Entities;

public class MerchantUser
{
    public Guid MerchantUserId { get; set; }

    public Guid UserId { get; set; }

    public Guid MerchantId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;

    public Merchant Merchant { get; set; } = null!;
}