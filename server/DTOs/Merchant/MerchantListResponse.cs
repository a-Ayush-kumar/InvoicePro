namespace server.DTOs.Merchant;

public class MerchantListResponse
{
    public List<MerchantResponse> Items { get; set; } = new();

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}