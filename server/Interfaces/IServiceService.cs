using server.DTOs.Service;

namespace server.Interfaces;

public interface IServiceService
{
    Task<ServiceResponse?> CreateAsync(
        Guid merchantId,
        CreateServiceRequest request);

    Task<ServiceResponse?> GetByIdAsync(
        Guid serviceId);

    Task<ServiceListResponse> GetByMerchantAsync(
        Guid merchantId,
        ServiceQueryRequest query);

    Task<ServiceResponse?> UpdateAsync(
        Guid serviceId,
        UpdateServiceRequest request);

    Task<ServiceResponse?> DeactivateAsync(
        Guid serviceId);

    Task<ServiceResponse?> ActivateAsync(
        Guid serviceId);
}