namespace WebAPITemplate.Shared.Models.Requests.User;

public class UserAuthenticationRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}