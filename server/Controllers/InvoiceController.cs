using Microsoft.AspNetCore.Mvc;
using server.DTOs.Invoice;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceRequest request)
    {
        var result = await _invoiceService.CreateAsync(request);

        if (result == null)
            return BadRequest();

        return Ok(result);
    }

    [HttpGet("{invoiceId:guid}")]
    public async Task<IActionResult> GetById(Guid invoiceId)
    {
        var result = await _invoiceService.GetByIdAsync(invoiceId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
    [HttpGet("merchant/{merchantId:guid}")]
    public async Task<IActionResult> GetByMerchant(
        Guid merchantId,
        [FromQuery] InvoiceQueryRequest query)
    {
        var result = await _invoiceService.GetByMerchantAsync(
            merchantId,
            query);

        return Ok(result);
    }
    [HttpPut("{invoiceId:guid}")]
    public async Task<IActionResult> Update(
        Guid invoiceId,
    [FromBody] UpdateInvoiceRequest request)
    {   
        var result = await _invoiceService.UpdateAsync(
            invoiceId,
            request);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}