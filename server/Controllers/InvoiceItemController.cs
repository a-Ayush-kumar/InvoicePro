using Microsoft.AspNetCore.Mvc;
using server.DTOs.InvoiceItem;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/invoice-items")]
public class InvoiceItemController : ControllerBase
{
    private readonly IInvoiceItemService _invoiceItemService;

    public InvoiceItemController(
        IInvoiceItemService invoiceItemService)
    {
        _invoiceItemService = invoiceItemService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateInvoiceItemRequest request)
    {
        var result = await _invoiceItemService.CreateAsync(request);

        if (result == null)
            return BadRequest();

        return Ok(result);
    }

    [HttpGet("{invoiceItemId:guid}")]
    public async Task<IActionResult> GetById(
        Guid invoiceItemId)
    {
        var result = await _invoiceItemService.GetByIdAsync(invoiceItemId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}