using server.DTOs.Customer;

namespace server.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponse> CreateAsync(
        CreateCustomerRequest request);

    Task<CustomerResponse?> GetByIdAsync(
        Guid customerId);

    Task<CustomerListResponse> GetAllAsync(
    CustomerQueryRequest query);

    Task<CustomerResponse?> UpdateAsync(
        Guid customerId,
        UpdateCustomerRequest request);

    Task<CustomerResponse?> DeactivateAsync(
        Guid customerId);

    Task<CustomerResponse?> ActivateAsync(
        Guid customerId);
}