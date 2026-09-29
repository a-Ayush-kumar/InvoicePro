using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.Merchant;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class MerchantService : IMerchantService
{
    private readonly InvoiceDbContext _db;

    public MerchantService(InvoiceDbContext db)
    {
        _db = db;
    }

    public async Task<MerchantResponse> CreateAsync(
        CreateMerchantRequest request,
        Guid adminId)
    {
        var merchant = new Merchant
        {
            AdminId = adminId,

            LegalName = request.LegalName,
            DisplayName = request.DisplayName,
            Email = request.Email,
            Phone = request.Phone,
            Gstin = request.Gstin,

            Address = request.Address,
            District = request.District,
            State = request.State,
            PostalCode = request.PostalCode,
            Country = request.Country,

            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Merchants.Add(merchant);
        await _db.SaveChangesAsync();

        return MapToResponse(merchant);
    }

    public async Task<MerchantResponse?> GetByIdAsync(
        Guid merchantId)
    {
        var merchant = await _db.Merchants
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MerchantId == merchantId);

        if (merchant == null)
            return null;

        return MapToResponse(merchant);
    }

    public async Task<MerchantListResponse> GetAllAsync(
        MerchantQueryRequest query)
    {
        var merchants = _db.Merchants
            .AsNoTracking()
            .AsQueryable();

        // Filter by active status
        if (query.IsActive.HasValue)
        {
            merchants = merchants.Where(
                m => m.IsActive == query.IsActive.Value);
        }

        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            merchants = merchants.Where(m =>
                m.LegalName.Contains(search) ||
                m.DisplayName.Contains(search) ||
                m.Email.Contains(search) ||
                m.Phone.Contains(search) ||
                (m.Gstin != null && m.Gstin.Contains(search)));
        }

        // Pagination normalization
        var page = query.Page < 1 ? 1 : query.Page;

        var pageSize = query.PageSize < 1
            ? 20
            : query.PageSize > 100
                ? 100
                : query.PageSize;

        // Total records before pagination
        var totalCount = await merchants.CountAsync();

        // Sorting
        var sortBy = query.SortBy?.ToLower() ?? "displayname";
        var sortDescending =
            query.SortOrder?.ToLower() == "desc";

        merchants = sortBy switch
        {
            "legalname" => sortDescending
                ? merchants.OrderByDescending(m => m.LegalName)
                : merchants.OrderBy(m => m.LegalName),

            "email" => sortDescending
                ? merchants.OrderByDescending(m => m.Email)
                : merchants.OrderBy(m => m.Email),

            "createdat" => sortDescending
                ? merchants.OrderByDescending(m => m.CreatedAt)
                : merchants.OrderBy(m => m.CreatedAt),

            "updatedat" => sortDescending
                ? merchants.OrderByDescending(m => m.UpdatedAt)
                : merchants.OrderBy(m => m.UpdatedAt),

            _ => sortDescending
                ? merchants.OrderByDescending(m => m.DisplayName)
                : merchants.OrderBy(m => m.DisplayName)
        };

        // Pagination
        var items = await merchants
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MerchantResponse
            {
                MerchantId = m.MerchantId,
                AdminId = m.AdminId,

                LegalName = m.LegalName,
                DisplayName = m.DisplayName,
                Email = m.Email,
                Phone = m.Phone,
                Gstin = m.Gstin,

                Address = m.Address,
                District = m.District,
                State = m.State,
                PostalCode = m.PostalCode,
                Country = m.Country,

                IsActive = m.IsActive,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            })
            .ToListAsync();

        var totalPages =
            (int)Math.Ceiling((double)totalCount / pageSize);

        return new MerchantListResponse
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<MerchantResponse?> UpdateAsync(
        Guid merchantId,
        UpdateMerchantRequest request)
    {
        var merchant = await _db.Merchants
            .FirstOrDefaultAsync(m => m.MerchantId == merchantId);

        if (merchant == null)
            return null;

        merchant.LegalName = request.LegalName;
        merchant.DisplayName = request.DisplayName;
        merchant.Email = request.Email;
        merchant.Phone = request.Phone;
        merchant.Gstin = request.Gstin;

        merchant.Address = request.Address;
        merchant.District = request.District;
        merchant.State = request.State;
        merchant.PostalCode = request.PostalCode;
        merchant.Country = request.Country;

        merchant.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapToResponse(merchant);
    }

    public async Task<MerchantResponse?> DeactivateAsync(
        Guid merchantId)
    {
        var merchant = await _db.Merchants
            .FirstOrDefaultAsync(m => m.MerchantId == merchantId);

        if (merchant == null)
            return null;

        // Already inactive — no database update required
        if (!merchant.IsActive)
            return MapToResponse(merchant);

        merchant.IsActive = false;
        merchant.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapToResponse(merchant);
    }

    public async Task<MerchantResponse?> ActivateAsync(
        Guid merchantId)
    {
        var merchant = await _db.Merchants
            .FirstOrDefaultAsync(m => m.MerchantId == merchantId);

        if (merchant == null)
            return null;

        // Already active — no database update required
        if (merchant.IsActive)
            return MapToResponse(merchant);

        merchant.IsActive = true;
        merchant.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapToResponse(merchant);
    }

    private static MerchantResponse MapToResponse(
        Merchant merchant)
    {
        return new MerchantResponse
        {
            MerchantId = merchant.MerchantId,
            AdminId = merchant.AdminId,

            LegalName = merchant.LegalName,
            DisplayName = merchant.DisplayName,
            Email = merchant.Email,
            Phone = merchant.Phone,
            Gstin = merchant.Gstin,

            Address = merchant.Address,
            District = merchant.District,
            State = merchant.State,
            PostalCode = merchant.PostalCode,
            Country = merchant.Country,

            IsActive = merchant.IsActive,
            CreatedAt = merchant.CreatedAt,
            UpdatedAt = merchant.UpdatedAt
        };
    }
}