using Microsoft.AspNetCore.Mvc;
using server.DTOs.Customer;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(
        CreateCustomerRequest request)
    {
        var customer = await _customerService.CreateAsync(request);

        return CreatedAtAction(
            nameof(Create),
            new { id = customer.CustomerId },
            customer);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> GetById(
        Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll(
        [FromQuery] CustomerQueryRequest query)
    {
        var customers = await _customerService.GetAllAsync(query);

        return Ok(customers);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> Update(
    Guid id,
    UpdateCustomerRequest request)
    {
        var customer = await _customerService.UpdateAsync(
            id,
            request);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<ActionResult<CustomerResponse>> Deactivate(
        Guid id)
    {
        var customer = await _customerService.DeactivateAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<ActionResult<CustomerResponse>> Activate(
        Guid id)
    {
        var customer = await _customerService.ActivateAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }
}