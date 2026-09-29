namespace server.Entities;

public class Customer{
    public Guid CustomerId {get; set;}
    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public string Name {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Phone {get; set;} = string.Empty;
    public string? Gstin {get; set;} = string.Empty;
    public string BillingAddress {get; set;} = string.Empty;
    public string ShippingAddress {get; set;} = string.Empty;
    public string District {get; set;} = string.Empty;
    public string State {get; set;} = string.Empty;
    public string PostalCode {get; set;} = string.Empty;
    public string Country {get; set;} = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}

    public ICollection<MerchantCustomer> MerchantCustomers {get; set;} = new List<MerchantCustomer>();
}