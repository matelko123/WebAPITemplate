using WebAPITemplate.Shared.Models.Requests.User;
using WebAPITemplate.Shared.Models.Responses;

namespace WebAPITemplate.API.Services;

public interface IAuthenticationService
{
    Task<UserAuthenticationResponse> AuthenticateAsync(UserAuthenticationRequest request);
    Task<UserAuthenticationResponse> RegisterAsync(UserRegistrationRequest request);
}