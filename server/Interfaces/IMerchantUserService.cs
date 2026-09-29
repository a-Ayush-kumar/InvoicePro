using server.DTOs.MerchantUser;

namespace server.Interfaces;

public interface IMerchantUserService
{
    Task<MerchantUserResponse?> CreateAsync(
        Guid merchantId,
        CreateMerchantUserRequest request);

    Task<MerchantUserResponse?> GetByIdAsync(
        Guid merchantUserId);

    Task<MerchantUserListResponse> GetByMerchantAsync(
        Guid merchantId,
        MerchantUserQueryRequest query);

    Task<MerchantUserResponse?> UpdateAsync(
        Guid merchantUserId,
        UpdateMerchantUserRequest request);

    Task<MerchantUserResponse?> DeactivateAsync(
        Guid merchantUserId);

    Task<MerchantUserResponse?> ActivateAsync(
        Guid merchantUserId);
}