using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.MerchantUser;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class MerchantUserService : IMerchantUserService
{
    private readonly InvoiceDbContext _db;

    public MerchantUserService(InvoiceDbContext db)
    {
        _db = db;
    }

    public async Task<MerchantUserResponse?> CreateAsync(
        Guid merchantId,
        CreateMerchantUserRequest request)
    {
        var merchantExists = await _db.Merchants
            .AnyAsync(m => m.MerchantId == merchantId);

        if (!merchantExists)
            return null;

        var userExists = await _db.Users
            .AnyAsync(u => u.UserId == request.UserId);

        if (!userExists)
            return null;

        var existingMembership = await _db.MerchantUsers
            .FirstOrDefaultAsync(mu =>
                mu.UserId == request.UserId &&
                mu.MerchantId == merchantId);

        if (existingMembership != null)
        {
            if (!existingMembership.IsActive)
            {
                existingMembership.IsActive = true;
                existingMembership.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();
            }

            return MapToResponse(existingMembership);
        }

        var activeMembershipExists = await _db.MerchantUsers
            .AnyAsync(mu =>
                mu.UserId == request.UserId &&
                mu.IsActive);

        if (activeMembershipExists)
            return null;

        var merchantUser = new MerchantUser
        {
            UserId = request.UserId,
            MerchantId = merchantId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.MerchantUsers.Add(merchantUser);

        await _db.SaveChangesAsync();

        return MapToResponse(merchantUser);
    }

    public async Task<MerchantUserResponse?> GetByIdAsync(
        Guid merchantUserId)
    {
        var merchantUser = await _db.MerchantUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(mu =>
                mu.MerchantUserId == merchantUserId);

        if (merchantUser == null)
            return null;

        return MapToResponse(merchantUser);
    }

    public async Task<MerchantUserListResponse> GetByMerchantAsync(
        Guid merchantId,
        MerchantUserQueryRequest query)
    {
        var users = _db.MerchantUsers
            .AsNoTracking()
            .Where(mu => mu.MerchantId == merchantId);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            users = users.Where(mu =>
                mu.User.Name.Contains(search) ||
                mu.User.Email.Contains(search));
        }

        var page = query.Page < 1 ? 1 : query.Page;

        var pageSize = query.PageSize < 1
            ? 20
            : query.PageSize > 100
                ? 100
                : query.PageSize;

        var totalCount = await users.CountAsync();

        var sortBy = query.SortBy?.ToLower() ?? "name";

        users = sortBy switch
        {
            "email" => query.Descending
                ? users.OrderByDescending(mu => mu.User.Email)
                : users.OrderBy(mu => mu.User.Email),

            "createdat" => query.Descending
                ? users.OrderByDescending(mu => mu.CreatedAt)
                : users.OrderBy(mu => mu.CreatedAt),

            "updatedat" => query.Descending
                ? users.OrderByDescending(mu => mu.UpdatedAt)
                : users.OrderBy(mu => mu.UpdatedAt),

            _ => query.Descending
                ? users.OrderByDescending(mu => mu.User.Name)
                : users.OrderBy(mu => mu.User.Name)
        };

        var items = await users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(mu => new MerchantUserResponse
            {
                MerchantUserId = mu.MerchantUserId,
                UserId = mu.UserId,
                MerchantId = mu.MerchantId,
                IsActive = mu.IsActive,
                CreatedAt = mu.CreatedAt,
                UpdatedAt = mu.UpdatedAt
            })
            .ToListAsync();

        var totalPages =
            (int)Math.Ceiling((double)totalCount / pageSize);

        return new MerchantUserListResponse
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<MerchantUserResponse?> UpdateAsync(
        Guid merchantUserId,
        UpdateMerchantUserRequest request)
    {
        var merchantUser = await _db.MerchantUsers
            .Include(mu => mu.User)
            .FirstOrDefaultAsync(mu =>
                mu.MerchantUserId == merchantUserId);

        if (merchantUser == null)
            return null;

        merchantUser.User.Name = request.Name;
        merchantUser.User.Email = request.Email;
        merchantUser.User.UpdatedAt = DateTime.UtcNow;

        merchantUser.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapToResponse(merchantUser);
    }

    public async Task<MerchantUserResponse?> DeactivateAsync(
        Guid merchantUserId)
    {
        var merchantUser = await _db.MerchantUsers
            .FirstOrDefaultAsync(mu =>
                mu.MerchantUserId == merchantUserId);

        if (merchantUser == null)
            return null;

        if (!merchantUser.IsActive)
            return MapToResponse(merchantUser);

        merchantUser.IsActive = false;
        merchantUser.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapToResponse(merchantUser);
    }

    public async Task<MerchantUserResponse?> ActivateAsync(
        Guid merchantUserId)
    {
        var merchantUser = await _db.MerchantUsers
            .FirstOrDefaultAsync(mu =>
                mu.MerchantUserId == merchantUserId);

        if (merchantUser == null)
            return null;

        if (merchantUser.IsActive)
            return MapToResponse(merchantUser);

        var anotherActiveMembershipExists =
            await _db.MerchantUsers.AnyAsync(mu =>
                mu.UserId == merchantUser.UserId &&
                mu.MerchantUserId != merchantUserId &&
                mu.IsActive);

        if (anotherActiveMembershipExists)
            return null;

        merchantUser.IsActive = true;
        merchantUser.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapToResponse(merchantUser);
    }

    private static MerchantUserResponse MapToResponse(
        MerchantUser merchantUser)
    {
        return new MerchantUserResponse
        {
            MerchantUserId = merchantUser.MerchantUserId,
            UserId = merchantUser.UserId,
            MerchantId = merchantUser.MerchantId,
            IsActive = merchantUser.IsActive,
            CreatedAt = merchantUser.CreatedAt,
            UpdatedAt = merchantUser.UpdatedAt
        };
    }
}