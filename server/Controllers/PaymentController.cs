using Microsoft.AspNetCore.Mvc;
using server.DTOs.Payment;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePaymentRequest request)
    {
        var result = await _paymentService.CreateAsync(request);

        if (result == null)
            return BadRequest();

        return Ok(result);
    }

    [HttpGet("{paymentId:guid}")]
    public async Task<IActionResult> GetById(
        Guid paymentId)
    {
        var result = await _paymentService.GetByIdAsync(paymentId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}