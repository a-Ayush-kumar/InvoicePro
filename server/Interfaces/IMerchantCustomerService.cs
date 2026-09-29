using server.DTOs.MerchantCustomer;
using server.Services;

namespace server.Interfaces;

public interface IMerchantCustomerService
{
    Task<(MerchantCustomerCreateResult Result, MerchantCustomerResponse? Response)>
        CreateAsync(Guid merchantId, Guid customerId);

    Task<List<MerchantCustomerListItemResponse>?> GetMerchantCustomersAsync(Guid merchantId);

    Task<MerchantCustomerResponse?> GetByIdAsync(Guid merchantId, Guid customerId);

    Task<MerchantCustomerResponse?> DeactivateAsync(Guid merchantId, Guid customerId);

    Task<MerchantCustomerResponse?> ActivateAsync(Guid merchantId, Guid customerId);

    Task<List<CustomerMerchantListItemResponse>?> GetCustomerMerchantsAsync(Guid customerId);
}