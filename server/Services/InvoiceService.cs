using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.Invoice;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class InvoiceService : IInvoiceService
{
    private readonly InvoiceDbContext _context;

    public InvoiceService(InvoiceDbContext context)
    {
        _context = context;
    }

    public async Task<InvoiceResponse?> CreateAsync(
        CreateInvoiceRequest request)
    {
        var merchantExists = await _context.Merchants
            .AnyAsync(m => m.MerchantId == request.MerchantId);

        if (!merchantExists)
            return null;

        var customerBelongsToMerchant = await _context.MerchantCustomers
            .AnyAsync(mc =>
                mc.MerchantId == request.MerchantId &&
                mc.CustomerId == request.CustomerId &&
                mc.IsActive);

        if (!customerBelongsToMerchant)
            return null;

        var createdByMerchantUser = await _context.MerchantUsers
            .FirstOrDefaultAsync(mu =>
                mu.MerchantUserId == request.CreatedByMerchantUserId &&
                mu.MerchantId == request.MerchantId &&
                mu.IsActive);

        if (createdByMerchantUser == null)
            return null;

        var invoice = new Invoice
        {
            MerchantId = request.MerchantId,
            CustomerId = request.CustomerId,
            CreatedByMerchantUserId = request.CreatedByMerchantUserId,

            InvoiceNumber = request.InvoiceNumber,
            IssueDate = request.IssueDate,
            DueDate = request.DueDate,

            PlaceOfSupply = request.PlaceOfSupply,
            ReverseCharge = request.ReverseCharge,

            Currency = request.Currency,

            Subtotal = request.Subtotal,
            DiscountAmount = request.DiscountAmount,
            TaxableAmount = request.TaxableAmount,
            TotalAmount = request.TotalAmount,

            Status = request.Status,
            Notes = request.Notes,
            TermsAndCondition = request.TermsAndCondition,

            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Invoices.Add(invoice);

        await _context.SaveChangesAsync();

        return MapToResponse(invoice);
    }

   public async Task<InvoiceDetailResponse?> GetByIdAsync(
    Guid invoiceId)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.InvoiceItems)
                .ThenInclude(ii => ii.Service)
            .Include(i => i.Payments)
                .ThenInclude(p => p.RecordedByMerchantUser)
            .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

        if (invoice == null)
            return null;

        return new InvoiceDetailResponse
        {
            InvoiceId = invoice.InvoiceId,
            MerchantId = invoice.MerchantId,

            Customer = new InvoiceCustomerResponse
            {
                CustomerId = invoice.Customer.CustomerId,
                Name = invoice.Customer.Name,
                Email = invoice.Customer.Email,
                Phone = invoice.Customer.Phone,
                Gstin = invoice.Customer.Gstin ?? string.Empty,
                BillingAddress = invoice.Customer.BillingAddress,
                ShippingAddress = invoice.Customer.ShippingAddress,
                District = invoice.Customer.District,
                State = invoice.Customer.State,
                PostalCode = invoice.Customer.PostalCode,
                Country = invoice.Customer.Country
            },

            CreatedByMerchantUserId = invoice.CreatedByMerchantUserId,
            UpdatedByMerchantUserId = invoice.UpdatedByMerchantUserId,

            InvoiceNumber = invoice.InvoiceNumber,
            IssueDate = invoice.IssueDate,
            DueDate = invoice.DueDate,

            PlaceOfSupply = invoice.PlaceOfSupply,
            ReverseCharge = invoice.ReverseCharge,

            Currency = invoice.Currency,
            Subtotal = invoice.Subtotal,
            DiscountAmount = invoice.DiscountAmount,
            TaxableAmount = invoice.TaxableAmount,
            TotalAmount = invoice.TotalAmount,

            Status = invoice.Status,
            Notes = invoice.Notes,
            TermsAndCondition = invoice.TermsAndCondition,

            CreatedAt = invoice.CreatedAt,
            UpdatedAt = invoice.UpdatedAt,

            InvoiceItems = invoice.InvoiceItems
                .Select(ii => new InvoiceDetailItemResponse
                {
                    InvoiceItemId = ii.InvoiceItemId,
                    ServiceId = ii.ServiceId,

                    Description = ii.Description,
                    HsnSacCode = ii.HsnSacCode,
                    Quantity = ii.Quantity,
                    Unit = ii.Unit,
                    UnitPrice = ii.UnitPrice,
                    TaxRate = ii.TaxRate,
                    DiscountAmount = ii.DiscountAmount,
                    TaxableAmount = ii.TaxableAmount,
                    TaxAmount = ii.TaxAmount,
                    LineTotal = ii.LineTotal,

                    ServiceName = ii.Service.Name
                })
                .ToList(),

            Payments = invoice.Payments
                .Select(p => new InvoiceDetailPaymentResponse
                {
                    PaymentId = p.PaymentId,
                    Amount = p.Amount,
                    TransactionId = p.TransactionId,
                    TransactionDate = p.TransactionDate,

                    PayerName = p.PayerName,
                    BankName = p.BankName,
                    PaymentMethod = p.PaymentMethod,

                    Status = p.Status,

                    RecordedByMerchantUserId =
                        p.RecordedByMerchantUserId,

                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToList()
        };
    }

    private static InvoiceResponse MapToResponse(Invoice invoice)
    {
        return new InvoiceResponse
        {
            InvoiceId = invoice.InvoiceId,
            MerchantId = invoice.MerchantId,
            CustomerId = invoice.CustomerId,
            CreatedByMerchantUserId = invoice.CreatedByMerchantUserId,
            UpdatedByMerchantUserId = invoice.UpdatedByMerchantUserId,

            InvoiceNumber = invoice.InvoiceNumber,
            IssueDate = invoice.IssueDate,
            DueDate = invoice.DueDate,

            PlaceOfSupply = invoice.PlaceOfSupply,
            ReverseCharge = invoice.ReverseCharge,

            Currency = invoice.Currency,

            Subtotal = invoice.Subtotal,
            DiscountAmount = invoice.DiscountAmount,
            TaxableAmount = invoice.TaxableAmount,
            TotalAmount = invoice.TotalAmount,

            Status = invoice.Status,
            Notes = invoice.Notes,
            TermsAndCondition = invoice.TermsAndCondition,

            CreatedAt = invoice.CreatedAt,
            UpdatedAt = invoice.UpdatedAt
        };
    }
    public async Task<InvoiceListResponse> GetByMerchantAsync(
    Guid merchantId,
    InvoiceQueryRequest query)
    {
        var invoices = _context.Invoices
            .Where(i => i.MerchantId == merchantId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim().ToLower();

            invoices = invoices.Where(i =>
                i.InvoiceNumber.ToLower().Contains(search) ||
                i.PlaceOfSupply.ToLower().Contains(search) ||
                i.Notes.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(query.Status) &&
            Enum.TryParse<InvoiceStatus>(
                query.Status,
                true,
                out var status))
        {
            invoices = invoices.Where(i => i.Status == status);
        }

        invoices = query.SortBy.ToLower() switch
        {
            "invoicenumber" => query.Descending
                ? invoices.OrderByDescending(i => i.InvoiceNumber)
                : invoices.OrderBy(i => i.InvoiceNumber),

            "issuedate" => query.Descending
                ? invoices.OrderByDescending(i => i.IssueDate)
                : invoices.OrderBy(i => i.IssueDate),

            "duedate" => query.Descending
                ? invoices.OrderByDescending(i => i.DueDate)
                : invoices.OrderBy(i => i.DueDate),

            "totalamount" => query.Descending
                ? invoices.OrderByDescending(i => i.TotalAmount)
                : invoices.OrderBy(i => i.TotalAmount),

            "status" => query.Descending
                ? invoices.OrderByDescending(i => i.Status)
                : invoices.OrderBy(i => i.Status),

            "updatedat" => query.Descending
                ? invoices.OrderByDescending(i => i.UpdatedAt)
                : invoices.OrderBy(i => i.UpdatedAt),

            _ => query.Descending
                ? invoices.OrderByDescending(i => i.CreatedAt)
                : invoices.OrderBy(i => i.CreatedAt)
        };

        var totalCount = await invoices.CountAsync();

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;

        var items = await invoices
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new InvoiceListResponse
        {
            Items = items.Select(MapToResponse).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize)
        };
    }
    public async Task<InvoiceResponse?> UpdateAsync(
        Guid invoiceId,
        UpdateInvoiceRequest request)
    {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

        if (invoice == null)
            return null;

        var customerBelongsToMerchant = await _context.MerchantCustomers
            .AnyAsync(mc =>
                mc.MerchantId == invoice.MerchantId &&
                mc.CustomerId == request.CustomerId &&
                mc.IsActive);

        if (!customerBelongsToMerchant)
            return null;

        var updatedByMerchantUser = await _context.MerchantUsers
            .FirstOrDefaultAsync(mu =>
                mu.MerchantUserId == request.UpdatedByMerchantUserId &&
                mu.MerchantId == invoice.MerchantId &&
                mu.IsActive);

        if (updatedByMerchantUser == null)
            return null;

        invoice.CustomerId = request.CustomerId;
        invoice.UpdatedByMerchantUserId = request.UpdatedByMerchantUserId;

        invoice.InvoiceNumber = request.InvoiceNumber;
        invoice.IssueDate = request.IssueDate;
        invoice.DueDate = request.DueDate;

        invoice.PlaceOfSupply = request.PlaceOfSupply;
        invoice.ReverseCharge = request.ReverseCharge;

        invoice.Currency = request.Currency;

        invoice.Subtotal = request.Subtotal;
        invoice.DiscountAmount = request.DiscountAmount;
        invoice.TaxableAmount = request.TaxableAmount;
        invoice.TotalAmount = request.TotalAmount;

        invoice.Status = request.Status;
        invoice.Notes = request.Notes;
        invoice.TermsAndCondition = request.TermsAndCondition;

        invoice.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(invoice);
    }
}