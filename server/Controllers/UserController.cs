using Microsoft.AspNetCore.Mvc;
using server.DTOs.User;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateUserRequest request)
    {
        var result = await _userService.CreateAsync(request);

        return Ok(result);
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetById(
        Guid userId)
    {
        var result = await _userService.GetByIdAsync(userId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> Update(
        Guid userId,
        UpdateUserRequest request)
    {
        var result = await _userService.UpdateAsync(
            userId,
            request);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}