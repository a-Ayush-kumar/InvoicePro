namespace server.DTOs.MerchantUser;

public class MerchantUserListResponse
{
    public List<MerchantUserResponse> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}