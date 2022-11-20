using Microsoft.AspNetCore.Mvc;
using WebAPITemplate.API.Services;
using WebAPITemplate.Shared.Models.Requests.User;
using WebAPITemplate.Shared.Models.Responses;

namespace WebAPITemplate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("/Authenticate")]
    public async Task<ActionResult<UserAuthenticationResponse>> Authenticate([FromBody] UserAuthenticationRequest user)
        => Ok(await _authenticationService.AuthenticateAsync(user));
    
    [HttpPost("/Register")]
    public async Task<ActionResult<UserAuthenticationResponse>> Register([FromBody] UserRegistrationRequest user)
        => Ok(await _authenticationService.RegisterAsync(user));
}