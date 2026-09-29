namespace server.Entities;

public class MerchantCustomer{
    public Guid MerchantId { get; set; }
    public Guid CustomerId { get; set; }
    
    //creating merchant and customer permanent relation
    public bool IsActive { get; set; }

    //above one is the composite primary key Implememntation
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    //it is the navigation property, allows the MerchantCustomer to navigate to other two - Merchant and customer
    public Merchant Merchant { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}