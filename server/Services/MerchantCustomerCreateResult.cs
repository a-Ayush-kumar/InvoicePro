namespace server.Services;

public enum MerchantCustomerCreateResult
{
    Created,
    MerchantNotFound,
    CustomerNotFound,
    RelationshipAlreadyExists
}