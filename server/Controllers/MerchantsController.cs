using Microsoft.AspNetCore.Mvc;
using server.DTOs.Merchant;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MerchantsController : ControllerBase
{
    private readonly IMerchantService _merchantService;

    public MerchantsController(IMerchantService merchantService)
    {
        _merchantService = merchantService;
    }

    [HttpPost]
    public async Task<ActionResult<MerchantResponse>> Create(
        CreateMerchantRequest request)
    {
        // Temporary until authentication is implemented
        var adminId = Guid.Parse(
            "11111111-1111-1111-1111-111111111111");

        var merchant = await _merchantService.CreateAsync(
            request,
            adminId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = merchant.MerchantId },
            merchant);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MerchantResponse>> GetById(
        Guid id)
    {
        var merchant = await _merchantService.GetByIdAsync(id);

        if (merchant == null)
            return NotFound();

        return Ok(merchant);
    }

    [HttpGet]
    public async Task<ActionResult<MerchantListResponse>> GetAll(
        [FromQuery] MerchantQueryRequest query)
    {
        var merchants = await _merchantService.GetAllAsync(query);

        return Ok(merchants);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MerchantResponse>> Update(
        Guid id,
        UpdateMerchantRequest request)
    {
        var merchant = await _merchantService.UpdateAsync(
            id,
            request);

        if (merchant == null)
            return NotFound();

        return Ok(merchant);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<ActionResult<MerchantResponse>> Deactivate(
        Guid id)
    {
        var merchant = await _merchantService.DeactivateAsync(id);

        if (merchant == null)
            return NotFound();

        return Ok(merchant);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<ActionResult<MerchantResponse>> Activate(
        Guid id)
    {
        var merchant = await _merchantService.ActivateAsync(id);

        if (merchant == null)
            return NotFound();

        return Ok(merchant);
    }
}