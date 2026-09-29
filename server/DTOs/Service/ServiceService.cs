using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.Service;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class ServiceService : IServiceService
{
    private readonly InvoiceDbContext _db;

    public ServiceService(InvoiceDbContext db)
    {
        _db = db;
    }

    public async Task<ServiceResponse?> CreateAsync(
        Guid merchantId,
        CreateServiceRequest request)
    {
        var merchantExists = await _db.Merchants
            .AnyAsync(m => m.MerchantId == merchantId);

        if (!merchantExists)
            return null;

        var service = new Service
        {
            MerchantId = merchantId,
            Name = request.Name,
            Description = request.Description,
            HsnSacCode = request.HsnSacCode,
            DefaultUnit = request.DefaultUnit,
            DefaultUnitPrice = request.DefaultUnitPrice,
            DefaultTaxRate = request.DefaultTaxRate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Services.Add(service);
        await _db.SaveChangesAsync();

        return MapToResponse(service);
    }

    public async Task<ServiceResponse?> GetByIdAsync(
        Guid serviceId)
    {
        var service = await _db.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

        if (service == null)
            return null;

        return MapToResponse(service);
    }

    public async Task<ServiceListResponse> GetByMerchantAsync(
        Guid merchantId,
        ServiceQueryRequest query)
    {
        var services = _db.Services
            .AsNoTracking()
            .Where(s => s.MerchantId == merchantId);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            services = services.Where(s =>
                s.Name.Contains(search) ||
                s.Description.Contains(search) ||
                s.HsnSacCode.Contains(search));
        }

        var page = query.Page < 1 ? 1 : query.Page;

        var pageSize = query.PageSize < 1
            ? 20
            : query.PageSize > 100
                ? 100
                : query.PageSize;

        var totalCount = await services.CountAsync();

        var sortBy = query.SortBy?.ToLower() ?? "name";

        services = sortBy switch
        {
            "price" => query.Descending
                ? services.OrderByDescending(s => s.DefaultUnitPrice)
                : services.OrderBy(s => s.DefaultUnitPrice),

            "taxrate" => query.Descending
                ? services.OrderByDescending(s => s.DefaultTaxRate)
                : services.OrderBy(s => s.DefaultTaxRate),

            "createdat" => query.Descending
                ? services.OrderByDescending(s => s.CreatedAt)
                : services.OrderBy(s => s.CreatedAt),

            "updatedat" => query.Descending
                ? services.OrderByDescending(s => s.UpdatedAt)
                : services.OrderBy(s => s.UpdatedAt),

            _ => query.Descending
                ? services.OrderByDescending(s => s.Name)
                : services.OrderBy(s => s.Name)
        };

        var items = await services
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new ServiceResponse
            {
                ServiceId = s.ServiceId,
                MerchantId = s.MerchantId,
                Name = s.Name,
                Description = s.Description,
                HsnSacCode = s.HsnSacCode,
                DefaultUnit = s.DefaultUnit,
                DefaultUnitPrice = s.DefaultUnitPrice,
                DefaultTaxRate = s.DefaultTaxRate,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .ToListAsync();

        var totalPages =
            (int)Math.Ceiling((double)totalCount / pageSize);

        return new ServiceListResponse
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<ServiceResponse?> UpdateAsync(
        Guid serviceId,
        UpdateServiceRequest request)
    {
        var service = await _db.Services
            .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

        if (service == null)
            return null;

        service.Name = request.Name;
        service.Description = request.Description;
        service.HsnSacCode = request.HsnSacCode;
        service.DefaultUnit = request.DefaultUnit;
        service.DefaultUnitPrice = request.DefaultUnitPrice;
        service.DefaultTaxRate = request.DefaultTaxRate;
        service.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapToResponse(service);
    }

    public async Task<ServiceResponse?> DeactivateAsync(
        Guid serviceId)
    {
        var service = await _db.Services
            .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

        if (service == null)
            return null;

        if (!service.IsActive)
            return MapToResponse(service);

        service.IsActive = false;
        service.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapToResponse(service);
    }

    public async Task<ServiceResponse?> ActivateAsync(
        Guid serviceId)
    {
        var service = await _db.Services
            .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

        if (service == null)
            return null;

        if (service.IsActive)
            return MapToResponse(service);

        service.IsActive = true;
        service.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapToResponse(service);
    }

    private static ServiceResponse MapToResponse(Service service)
    {
        return new ServiceResponse
        {
            ServiceId = service.ServiceId,
            MerchantId = service.MerchantId,
            Name = service.Name,
            Description = service.Description,
            HsnSacCode = service.HsnSacCode,
            DefaultUnit = service.DefaultUnit,
            DefaultUnitPrice = service.DefaultUnitPrice,
            DefaultTaxRate = service.DefaultTaxRate,
            IsActive = service.IsActive,
            CreatedAt = service.CreatedAt,
            UpdatedAt = service.UpdatedAt
        };
    }
}