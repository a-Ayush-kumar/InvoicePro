namespace server.DTOs.Customer;

public class CustomerQueryRequest
{
    public bool? IsActive { get; set; }
    public string? Search { get; set; }
    public string SortBy { get; set; } = "name";
    public string SortOrder { get; set; } = "asc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}