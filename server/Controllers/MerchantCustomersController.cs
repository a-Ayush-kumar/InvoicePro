using Microsoft.AspNetCore.Mvc;
using server.DTOs.MerchantCustomer;
using server.Interfaces;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("api")]
public class MerchantCustomersController : ControllerBase
{
    private readonly IMerchantCustomerService _merchantCustomerService;

    public MerchantCustomersController(
        IMerchantCustomerService merchantCustomerService)
    {
        _merchantCustomerService = merchantCustomerService;
    }

    [HttpPost("Merchants/{merchantId:guid}/customers/{customerId:guid}")]
    public async Task<ActionResult<MerchantCustomerResponse>> Create(
        Guid merchantId,
        Guid customerId)
    {
        var result = await _merchantCustomerService.CreateAsync(
            merchantId,
            customerId);

        return result.Result switch
        {
            MerchantCustomerCreateResult.Created =>
                StatusCode(StatusCodes.Status201Created, result.Response),

            MerchantCustomerCreateResult.MerchantNotFound =>
                NotFound("Merchant not found."),

            MerchantCustomerCreateResult.CustomerNotFound =>
                NotFound("Customer not found."),

            MerchantCustomerCreateResult.RelationshipAlreadyExists =>
                Conflict("The merchant-customer relationship already exists."),

            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpGet("Merchants/{merchantId:guid}/customers")]
    public async Task<ActionResult<List<MerchantCustomerListItemResponse>>> GetMerchantCustomers(
        Guid merchantId)
    {
        var customers =
            await _merchantCustomerService.GetMerchantCustomersAsync(merchantId);

        if (customers == null)
        {
            return NotFound("Merchant not found.");
        }

        return Ok(customers);
    }

    [HttpGet("Merchants/{merchantId:guid}/customers/{customerId:guid}")]
    public async Task<ActionResult<MerchantCustomerResponse>> GetById(
        Guid merchantId,
        Guid customerId)
    {
        var relationship =
            await _merchantCustomerService.GetByIdAsync(
                merchantId,
                customerId);

        if (relationship == null)
        {
            return NotFound("Merchant-customer relationship not found.");
        }

        return Ok(relationship);
    }

    [HttpPatch("Merchants/{merchantId:guid}/customers/{customerId:guid}/deactivate")]
    public async Task<ActionResult<MerchantCustomerResponse>> Deactivate(
        Guid merchantId,
        Guid customerId)
    {
        var relationship =
            await _merchantCustomerService.DeactivateAsync(
                merchantId,
                customerId);

        if (relationship == null)
        {
            return NotFound("Merchant-customer relationship not found.");
        }

        return Ok(relationship);
    }

    [HttpPatch("Merchants/{merchantId:guid}/customers/{customerId:guid}/activate")]
    public async Task<ActionResult<MerchantCustomerResponse>> Activate(
        Guid merchantId,
        Guid customerId)
    {
        var relationship =
            await _merchantCustomerService.ActivateAsync(
                merchantId,
                customerId);

        if (relationship == null)
        {
            return NotFound("Merchant-customer relationship not found.");
        }

        return Ok(relationship);
    }

    [HttpGet("Customers/{customerId:guid}/merchants")]
    public async Task<ActionResult<List<CustomerMerchantListItemResponse>>> GetCustomerMerchants(
        Guid customerId)
    {
        var merchants =
            await _merchantCustomerService.GetCustomerMerchantsAsync(customerId);

        if (merchants == null)
        {
            return NotFound("Customer not found.");
        }

        return Ok(merchants);
    }
}