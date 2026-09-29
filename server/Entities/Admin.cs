namespace server.Entities;

public class Admin{
    public Guid AdminId {get; set;}
    public string Name {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Phone {get; set;} = string.Empty;
    public string PasswordHash {get; set;} = string.Empty;
    
    public ICollection<Merchant> Merchants {get; set;} = new List<Merchant>();
}