using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.User;
using server.Entities;
using server.Interfaces;

namespace server.Services;

public class UserService : IUserService
{
    private readonly InvoiceDbContext _db;

    public UserService(InvoiceDbContext db)
    {
        _db = db;
    }

    public async Task<UserResponse> CreateAsync(
        CreateUserRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = request.Password,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return MapToResponse(user);
    }

    public async Task<UserResponse?> GetByIdAsync(
        Guid userId)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
            return null;

        return MapToResponse(user);
    }

    public async Task<UserResponse?> UpdateAsync(
        Guid userId,
        UpdateUserRequest request)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
            return null;

        user.Name = request.Name;
        user.Email = request.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return MapToResponse(user);
    }

    private static UserResponse MapToResponse(
        User user)
    {
        return new UserResponse
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}