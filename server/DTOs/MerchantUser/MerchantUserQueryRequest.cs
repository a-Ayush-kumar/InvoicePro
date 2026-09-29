namespace server.DTOs.MerchantUser;

public class MerchantUserQueryRequest
{
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool Descending { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}