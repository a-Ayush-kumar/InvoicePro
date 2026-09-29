namespace server.DTOs.Invoice;

public class InvoiceQueryRequest
{
    public string? Search { get; set; }
    public string? Status { get; set; }

    public string SortBy { get; set; } = "createdAt";
    public bool Descending { get; set; } = true;

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}