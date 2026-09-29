using Microsoft.AspNetCore.Mvc;
using server.DTOs.MerchantUser;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/merchants/{merchantId}/users")]
public class MerchantUserController : ControllerBase
{
    private readonly IMerchantUserService _merchantUserService;

    public MerchantUserController(
        IMerchantUserService merchantUserService)
    {
        _merchantUserService = merchantUserService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        Guid merchantId,
        CreateMerchantUserRequest request)
    {
        var result = await _merchantUserService.CreateAsync(
            merchantId,
            request);

        if (result == null)
            return BadRequest(
                "Merchant or User does not exist, or the User already has an active merchant membership.");

        return Ok(result);
    }

    [HttpGet("{merchantUserId:guid}")]
    public async Task<IActionResult> GetById(
        Guid merchantId,
        Guid merchantUserId)
    {
        var result = await _merchantUserService
            .GetByIdAsync(merchantUserId);

        if (result == null)
            return NotFound();

        if (result.MerchantId != merchantId)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByMerchant(
        Guid merchantId,
        [FromQuery] MerchantUserQueryRequest query)
    {
        var result = await _merchantUserService
            .GetByMerchantAsync(merchantId, query);

        return Ok(result);
    }

    [HttpPut("{merchantUserId:guid}")]
    public async Task<IActionResult> Update(
        Guid merchantId,
        Guid merchantUserId,
        UpdateMerchantUserRequest request)
    {
        var existing = await _merchantUserService
            .GetByIdAsync(merchantUserId);

        if (existing == null ||
            existing.MerchantId != merchantId)
            return NotFound();

        var result = await _merchantUserService
            .UpdateAsync(merchantUserId, request);

        return Ok(result);
    }

    [HttpPatch("{merchantUserId:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid merchantId,
        Guid merchantUserId)
    {
        var existing = await _merchantUserService
            .GetByIdAsync(merchantUserId);

        if (existing == null ||
            existing.MerchantId != merchantId)
            return NotFound();

        var result = await _merchantUserService
            .DeactivateAsync(merchantUserId);

        return Ok(result);
    }

    [HttpPatch("{merchantUserId:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid merchantId,
        Guid merchantUserId)
    {
        var existing = await _merchantUserService
            .GetByIdAsync(merchantUserId);

        if (existing == null ||
            existing.MerchantId != merchantId)
            return NotFound();

        var result = await _merchantUserService
            .ActivateAsync(merchantUserId);

        if (result == null)
            return BadRequest(
                "User already has another active merchant membership.");

        return Ok(result);
    }
}