using server.DTOs.User;

namespace server.Interfaces;

public interface IUserService
{
    Task<UserResponse> CreateAsync(
        CreateUserRequest request);

    Task<UserResponse?> GetByIdAsync(
        Guid userId);

    Task<UserResponse?> UpdateAsync(
        Guid userId,
        UpdateUserRequest request);
}