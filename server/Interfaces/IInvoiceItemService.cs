using server.DTOs.InvoiceItem;

namespace server.Interfaces;

public interface IInvoiceItemService
{
    Task<InvoiceItemResponse?> CreateAsync(
        CreateInvoiceItemRequest request);

    Task<InvoiceItemResponse?> GetByIdAsync(
        Guid invoiceItemId);
}