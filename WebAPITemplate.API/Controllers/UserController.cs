using Microsoft.AspNetCore.Mvc;
using WebAPITemplate.API.Services;
using WebAPITemplate.Shared.Models.Requests.User;
using WebAPITemplate.Shared.Models.Responses;

namespace WebAPITemplate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll() 
        => Ok(await _userService.GetAll());

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserDto>> GetById([FromRoute] Guid userId)
        => Ok(await _userService.GetById(userId));
    
    [HttpGet("{username}")]
    public async Task<ActionResult<UserDto>> GetByUsername([FromRoute] string username)
        => Ok(await _userService.GetByUsername(username));

    [HttpPost]
    public async Task<ActionResult<UserDto>> Add([FromBody] UserAddRequest user)
    {
        UserDto createdUser = await _userService.Add(user);
        return Created($"/{createdUser.Id}", createdUser);
    }

    [HttpPut]
    public async Task<ActionResult<UserDto>> Update([FromBody] UserUpdateRequest user)
        => Ok(await _userService.Update(user));

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid userId)
        => Ok(await _userService.Delete(userId));
}