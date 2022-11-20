namespace WebAPITemplate.Shared.Models.Responses;

public class UserAuthenticationResponse
{
    public UserDto User { get; set; }
    public string Token { get; set; }
}