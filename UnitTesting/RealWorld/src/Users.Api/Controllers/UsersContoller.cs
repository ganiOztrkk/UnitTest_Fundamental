using Microsoft.AspNetCore.Mvc;
using Users.Api.DTOs;
using Users.Api.Services;

namespace Users.Api.Controllers;


[ApiController]
[Route("api/[controller]/[action]")]
public class UsersContoller(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;


    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userList = await _userService.GetAllAsync(cancellationToken);
        return Ok(userList);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto request, CancellationToken cancellationToken)
    {
        var result =await _userService.CreateAsync(request, cancellationToken);
        return Ok(new { Result = result});
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _userService.DeleteByIdAsync(id, cancellationToken);
        return Ok(new { Result = result});
    }
    
}