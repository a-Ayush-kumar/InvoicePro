using Microsoft.AspNetCore.Mvc;
using server.DTOs.Service;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/merchants/{merchantId}/services")]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServiceController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        Guid merchantId,
        CreateServiceRequest request)
    {
        var result = await _serviceService
            .CreateAsync(merchantId, request);

        if (result == null)
            return NotFound("Merchant does not exist.");

        return Ok(result);
    }

    [HttpGet("{serviceId:guid}")]
    public async Task<IActionResult> GetById(
        Guid merchantId,
        Guid serviceId)
    {
        var result = await _serviceService
            .GetByIdAsync(serviceId);

        if (result == null ||
            result.MerchantId != merchantId)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByMerchant(
        Guid merchantId,
        [FromQuery] ServiceQueryRequest query)
    {
        var result = await _serviceService
            .GetByMerchantAsync(merchantId, query);

        return Ok(result);
    }

    [HttpPut("{serviceId:guid}")]
    public async Task<IActionResult> Update(
        Guid merchantId,
        Guid serviceId,
        UpdateServiceRequest request)
    {
        var existing = await _serviceService
            .GetByIdAsync(serviceId);

        if (existing == null ||
            existing.MerchantId != merchantId)
            return NotFound();

        var result = await _serviceService
            .UpdateAsync(serviceId, request);

        return Ok(result);
    }

    [HttpPatch("{serviceId:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid merchantId,
        Guid serviceId)
    {
        var existing = await _serviceService
            .GetByIdAsync(serviceId);

        if (existing == null ||
            existing.MerchantId != merchantId)
            return NotFound();

        var result = await _serviceService
            .DeactivateAsync(serviceId);

        return Ok(result);
    }

    [HttpPatch("{serviceId:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid merchantId,
        Guid serviceId)
    {
        var existing = await _serviceService
            .GetByIdAsync(serviceId);

        if (existing == null ||
            existing.MerchantId != merchantId)
            return NotFound();

        var result = await _serviceService
            .ActivateAsync(serviceId);

        return Ok(result);
    }
}