using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.MerchantCustomer;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class MerchantCustomerService : IMerchantCustomerService
{
    private readonly InvoiceDbContext _context;

    public MerchantCustomerService(InvoiceDbContext context)
    {
        _context = context;
    }

    public async Task<(MerchantCustomerCreateResult Result, MerchantCustomerResponse? Response)>
        CreateAsync(Guid merchantId, Guid customerId)
    {
        var merchantExists = await _context.Merchants
            .AnyAsync(m => m.MerchantId == merchantId);

        if (!merchantExists)
        {
            return (MerchantCustomerCreateResult.MerchantNotFound, null);
        }

        var customerExists = await _context.Customers
            .AnyAsync(c => c.CustomerId == customerId);

        if (!customerExists)
        {
            return (MerchantCustomerCreateResult.CustomerNotFound, null);
        }

        var relationshipExists = await _context.MerchantCustomers
            .AnyAsync(mc =>
                mc.MerchantId == merchantId &&
                mc.CustomerId == customerId);

        if (relationshipExists)
        {
            return (MerchantCustomerCreateResult.RelationshipAlreadyExists, null);
        }

        var now = DateTime.UtcNow;

        var relationship = new MerchantCustomer
        {
            MerchantId = merchantId,
            CustomerId = customerId,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.MerchantCustomers.Add(relationship);

        await _context.SaveChangesAsync();

        return (MerchantCustomerCreateResult.Created, MapToResponse(relationship));
    }

    public async Task<List<MerchantCustomerListItemResponse>?> GetMerchantCustomersAsync(
        Guid merchantId)
    {
        var merchantExists = await _context.Merchants
            .AnyAsync(m => m.MerchantId == merchantId);

        if (!merchantExists)
        {
            return null;
        }

        return await _context.MerchantCustomers
            .AsNoTracking()
            .Where(mc => mc.MerchantId == merchantId)
            .Select(mc => new MerchantCustomerListItemResponse
            {
                CustomerId = mc.CustomerId,
                Name = mc.Customer.Name,
                Email = mc.Customer.Email,
                Phone = mc.Customer.Phone,
                Gstin = mc.Customer.Gstin,
                CustomerIsActive = mc.Customer.IsActive,
                RelationshipIsActive = mc.IsActive,
                RelationshipCreatedAt = mc.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<MerchantCustomerResponse?> GetByIdAsync(
        Guid merchantId,
        Guid customerId)
    {
        var relationship = await _context.MerchantCustomers
            .AsNoTracking()
            .FirstOrDefaultAsync(mc =>
                mc.MerchantId == merchantId &&
                mc.CustomerId == customerId);

        return relationship == null
            ? null
            : MapToResponse(relationship);
    }

    public async Task<MerchantCustomerResponse?> DeactivateAsync(
        Guid merchantId,
        Guid customerId)
    {
        var relationship = await _context.MerchantCustomers
            .FirstOrDefaultAsync(mc =>
                mc.MerchantId == merchantId &&
                mc.CustomerId == customerId);

        if (relationship == null)
        {
            return null;
        }

        if (!relationship.IsActive)
        {
            return MapToResponse(relationship);
        }

        relationship.IsActive = false;
        relationship.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(relationship);
    }

    public async Task<MerchantCustomerResponse?> ActivateAsync(
        Guid merchantId,
        Guid customerId)
    {
        var relationship = await _context.MerchantCustomers
            .FirstOrDefaultAsync(mc =>
                mc.MerchantId == merchantId &&
                mc.CustomerId == customerId);

        if (relationship == null)
        {
            return null;
        }

        if (relationship.IsActive)
        {
            return MapToResponse(relationship);
        }

        relationship.IsActive = true;
        relationship.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(relationship);
    }

    public async Task<List<CustomerMerchantListItemResponse>?> GetCustomerMerchantsAsync(
        Guid customerId)
    {
        var customerExists = await _context.Customers
            .AnyAsync(c => c.CustomerId == customerId);

        if (!customerExists)
        {
            return null;
        }

        return await _context.MerchantCustomers
            .AsNoTracking()
            .Where(mc => mc.CustomerId == customerId)
            .Select(mc => new CustomerMerchantListItemResponse
            {
                MerchantId = mc.MerchantId,
                LegalName = mc.Merchant.LegalName,
                DisplayName = mc.Merchant.DisplayName,
                Email = mc.Merchant.Email,
                Phone = mc.Merchant.Phone,
                MerchantIsActive = mc.Merchant.IsActive,
                RelationshipIsActive = mc.IsActive,
                RelationshipCreatedAt = mc.CreatedAt
            })
            .ToListAsync();
    }

    private static MerchantCustomerResponse MapToResponse(
        MerchantCustomer relationship)
    {
        return new MerchantCustomerResponse
        {
            MerchantId = relationship.MerchantId,
            CustomerId = relationship.CustomerId,
            IsActive = relationship.IsActive,
            CreatedAt = relationship.CreatedAt,
            UpdatedAt = relationship.UpdatedAt
        };
    }
}