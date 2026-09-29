using server.DTOs.Invoice;

namespace server.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceResponse?> CreateAsync(
        CreateInvoiceRequest request);

    Task<InvoiceDetailResponse?> GetByIdAsync(
        Guid invoiceId);

    Task<InvoiceListResponse> GetByMerchantAsync(
        Guid merchantId,
        InvoiceQueryRequest query);

    Task<InvoiceResponse?> UpdateAsync(
        Guid invoiceId,
        UpdateInvoiceRequest request);
}