using server.Data;
using server.DTOs.Customer;
using server.Entities;
using server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace server.Services;

public class CustomerService : ICustomerService
{
    private readonly InvoiceDbContext _db;

    public CustomerService(InvoiceDbContext db)
    {
        _db = db;
    }

    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request)
    {
    var customer = new Customer
    {
        Name = request.Name,
        Email = request.Email,
        Phone = request.Phone,
        Gstin = request.Gstin,
        BillingAddress = request.BillingAddress,
        ShippingAddress = request.ShippingAddress,
        District = request.District,
        State = request.State,
        PostalCode = request.PostalCode,
        Country = request.Country,

        IsActive = true,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
        };

    _db.Customers.Add(customer);

    await _db.SaveChangesAsync();

    return new CustomerResponse
    {
        CustomerId = customer.CustomerId,
        Name = customer.Name,
        Email = customer.Email,
        Phone = customer.Phone,
        Gstin = customer.Gstin,
        BillingAddress = customer.BillingAddress,
        ShippingAddress = customer.ShippingAddress,
        District = customer.District,
        State = customer.State,
        PostalCode = customer.PostalCode,
        Country = customer.Country,
        IsActive = customer.IsActive,
        CreatedAt = customer.CreatedAt,
        UpdatedAt = customer.UpdatedAt
        };
   }

    public async Task<CustomerResponse?> GetByIdAsync(
    Guid customerId)
    {
    var customer = await _db.Customers
        .FirstOrDefaultAsync(c => c.CustomerId == customerId);

    if (customer == null)
    {
        return null;
    }

    return new CustomerResponse
    {
        CustomerId = customer.CustomerId,
        Name = customer.Name,
        Email = customer.Email,
        Phone = customer.Phone,
        Gstin = customer.Gstin,
        BillingAddress = customer.BillingAddress,
        ShippingAddress = customer.ShippingAddress,
        District = customer.District,
        State = customer.State,
        PostalCode = customer.PostalCode,
        Country = customer.Country,
        IsActive = customer.IsActive,
        CreatedAt = customer.CreatedAt,
        UpdatedAt = customer.UpdatedAt
        };
    }
    public async Task<CustomerListResponse> GetAllAsync(
    CustomerQueryRequest query)
{
    var customers = _db.Customers
        .AsNoTracking()
        .AsQueryable();

    // Filter by active status
    if (query.IsActive.HasValue)
    {
        customers = customers.Where(
            c => c.IsActive == query.IsActive.Value);
    }

    // Search
    if (!string.IsNullOrWhiteSpace(query.Search))
    {
        var search = query.Search.Trim();

        customers = customers.Where(c =>
            c.Name.Contains(search) ||
            c.Email.Contains(search) ||
            c.Phone.Contains(search) ||
            (c.Gstin != null && c.Gstin.Contains(search)));
    }

    // Pagination validation
    var page = query.Page < 1 ? 1 : query.Page;

    var pageSize = query.PageSize switch
    {
        < 1 => 20,
        > 100 => 100,
        _ => query.PageSize
    };

    // Total records after filtering/search
    var totalCount = await customers.CountAsync();

    // Sorting
    customers = query.SortBy.ToLower() switch
    {
        "email" => query.SortOrder.ToLower() == "desc"
            ? customers.OrderByDescending(c => c.Email)
            : customers.OrderBy(c => c.Email),

        "createdat" => query.SortOrder.ToLower() == "desc"
            ? customers.OrderByDescending(c => c.CreatedAt)
            : customers.OrderBy(c => c.CreatedAt),

        _ => query.SortOrder.ToLower() == "desc"
            ? customers.OrderByDescending(c => c.Name)
            : customers.OrderBy(c => c.Name)
    };

    // Get only requested page
    var items = await customers
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(c => new CustomerResponse
        {
            CustomerId = c.CustomerId,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            Gstin = c.Gstin,
            BillingAddress = c.BillingAddress,
            ShippingAddress = c.ShippingAddress,
            District = c.District,
            State = c.State,
            PostalCode = c.PostalCode,
            Country = c.Country,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        })
        .ToListAsync();

    var totalPages = (int)Math.Ceiling(
        totalCount / (double)pageSize);

    return new CustomerListResponse
    {
        Items = items,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount,
        TotalPages = totalPages
    };
}

public async Task<CustomerResponse?> UpdateAsync(
    Guid customerId,
    UpdateCustomerRequest request)
{
    var customer = await _db.Customers
        .FirstOrDefaultAsync(c => c.CustomerId == customerId);

    if (customer == null)
    {
        return null;
    }

    customer.Name = request.Name;
    customer.Email = request.Email;
    customer.Phone = request.Phone;
    customer.Gstin = request.Gstin;
    customer.BillingAddress = request.BillingAddress;
    customer.ShippingAddress = request.ShippingAddress;
    customer.District = request.District;
    customer.State = request.State;
    customer.PostalCode = request.PostalCode;
    customer.Country = request.Country;

    customer.UpdatedAt = DateTime.UtcNow;

    await _db.SaveChangesAsync();

    return new CustomerResponse
    {
        CustomerId = customer.CustomerId,
        Name = customer.Name,
        Email = customer.Email,
        Phone = customer.Phone,
        Gstin = customer.Gstin,
        BillingAddress = customer.BillingAddress,
        ShippingAddress = customer.ShippingAddress,
        District = customer.District,
        State = customer.State,
        PostalCode = customer.PostalCode,
        Country = customer.Country,
        IsActive = customer.IsActive,
        CreatedAt = customer.CreatedAt,
        UpdatedAt = customer.UpdatedAt
    };
}

 public async Task<CustomerResponse?> DeactivateAsync(
    Guid customerId)
{
    var customer = await _db.Customers
        .FirstOrDefaultAsync(c => c.CustomerId == customerId);

    if (customer == null)
    {
        return null;
    }

    customer.IsActive = false;
    customer.UpdatedAt = DateTime.UtcNow;

    await _db.SaveChangesAsync();

    return new CustomerResponse
    {
        CustomerId = customer.CustomerId,
        Name = customer.Name,
        Email = customer.Email,
        Phone = customer.Phone,
        Gstin = customer.Gstin,
        BillingAddress = customer.BillingAddress,
        ShippingAddress = customer.ShippingAddress,
        District = customer.District,
        State = customer.State,
        PostalCode = customer.PostalCode,
        Country = customer.Country,
        IsActive = customer.IsActive,
        CreatedAt = customer.CreatedAt,
        UpdatedAt = customer.UpdatedAt
    };
}

public async Task<CustomerResponse?> ActivateAsync(
    Guid customerId)
{
    var customer = await _db.Customers
        .FirstOrDefaultAsync(c => c.CustomerId == customerId);

    if (customer == null)
    {
        return null;
    }

    customer.IsActive = true;
    customer.UpdatedAt = DateTime.UtcNow;

    await _db.SaveChangesAsync();

    return new CustomerResponse
    {
        CustomerId = customer.CustomerId,
        Name = customer.Name,
        Email = customer.Email,
        Phone = customer.Phone,
        Gstin = customer.Gstin,
        BillingAddress = customer.BillingAddress,
        ShippingAddress = customer.ShippingAddress,
        District = customer.District,
        State = customer.State,
        PostalCode = customer.PostalCode,
        Country = customer.Country,
        IsActive = customer.IsActive,
        CreatedAt = customer.CreatedAt,
        UpdatedAt = customer.UpdatedAt
    };
}
}