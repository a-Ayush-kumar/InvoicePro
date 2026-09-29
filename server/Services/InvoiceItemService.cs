using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.InvoiceItem;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class InvoiceItemService : IInvoiceItemService
{
    private readonly InvoiceDbContext _context;

    public InvoiceItemService(InvoiceDbContext context)
    {
        _context = context;
    }

    public async Task<InvoiceItemResponse?> CreateAsync(
        CreateInvoiceItemRequest request)
    {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.InvoiceId == request.InvoiceId);

        if (invoice == null)
            return null;

        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.ServiceId == request.ServiceId);

        if (service == null)
            return null;

        // Tenant boundary:
        // The service must belong to the same merchant as the invoice.
        if (service.MerchantId != invoice.MerchantId)
            return null;

        var invoiceItem = new InvoiceItem
        {
            InvoiceId = request.InvoiceId,
            ServiceId = request.ServiceId,

            Description = request.Description,
            HsnSacCode = request.HsnSacCode,
            Quantity = request.Quantity,

            Unit = request.Unit,
            UnitPrice = request.UnitPrice,
            TaxRate = request.TaxRate,
            DiscountAmount = request.DiscountAmount,
            TaxableAmount = request.TaxableAmount,
            TaxAmount = request.TaxAmount,
            LineTotal = request.LineTotal
        };

        _context.InvoiceItems.Add(invoiceItem);

        await _context.SaveChangesAsync();

        return MapToResponse(invoiceItem);
    }

    public async Task<InvoiceItemResponse?> GetByIdAsync(
        Guid invoiceItemId)
    {
        var invoiceItem = await _context.InvoiceItems
            .FirstOrDefaultAsync(ii => ii.InvoiceItemId == invoiceItemId);

        if (invoiceItem == null)
            return null;

        return MapToResponse(invoiceItem);
    }

    private static InvoiceItemResponse MapToResponse(
        InvoiceItem invoiceItem)
    {
        return new InvoiceItemResponse
        {
            InvoiceItemId = invoiceItem.InvoiceItemId,
            InvoiceId = invoiceItem.InvoiceId,
            ServiceId = invoiceItem.ServiceId,

            Description = invoiceItem.Description,
            HsnSacCode = invoiceItem.HsnSacCode,
            Quantity = invoiceItem.Quantity,

            Unit = invoiceItem.Unit,
            UnitPrice = invoiceItem.UnitPrice,
            TaxRate = invoiceItem.TaxRate,
            DiscountAmount = invoiceItem.DiscountAmount,
            TaxableAmount = invoiceItem.TaxableAmount,
            TaxAmount = invoiceItem.TaxAmount,
            LineTotal = invoiceItem.LineTotal
        };
    }
}