namespace server.Entities;

public class User
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<MerchantUser> MerchantUsers { get; set; }
        = new List<MerchantUser>();

    public ICollection<Customer> Customers { get; set; }
        = new List<Customer>();
}