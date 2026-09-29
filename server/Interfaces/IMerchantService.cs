using server.DTOs.Merchant;

namespace server.Interfaces;

public interface IMerchantService
{
    Task<MerchantResponse> CreateAsync(
        CreateMerchantRequest request,
        Guid adminId);

    Task<MerchantResponse?> GetByIdAsync(
        Guid merchantId);

    Task<MerchantListResponse> GetAllAsync(
        MerchantQueryRequest query);

    Task<MerchantResponse?> UpdateAsync(
        Guid merchantId,
        UpdateMerchantRequest request);

    Task<MerchantResponse?> DeactivateAsync(
        Guid merchantId);

    Task<MerchantResponse?> ActivateAsync(
        Guid merchantId);
}