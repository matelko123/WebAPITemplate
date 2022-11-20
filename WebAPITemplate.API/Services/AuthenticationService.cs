using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapster;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAPITemplate.API.Exceptions;
using WebAPITemplate.API.Repositories;
using WebAPITemplate.Shared.Models.Entities;
using WebAPITemplate.Shared.Models.Requests.User;
using WebAPITemplate.Shared.Models.Responses;
using WebAPITemplate.Shared.Models.Settings;

namespace WebAPITemplate.API.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly JSONWebTokensSettings _jwtSettings;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IOptions<JSONWebTokensSettings> jwtSettings, IUserRepository userRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
    }

    public async Task<UserAuthenticationResponse> AuthenticateAsync(UserAuthenticationRequest request)
    {
        User user = await _userRepository.GetByUsername(request.Username);
        
        if (!BCrypt.Net.BCrypt.Verify(user.Password, request.Password))
            throw new BadRequestException("Username or password is incorrect");
        
        if(!user.IsEnabled)
            throw new UnauthorizedAccessException("Your account is blocked.");
        
        JwtSecurityToken jwtSecurityToken = GenerateToken(user);
        
        UserAuthenticationResponse response = new ()
        {
            User = user.Adapt<UserDto>(),
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
        };

        return response;
    }

    public async Task<UserAuthenticationResponse> RegisterAsync(UserRegistrationRequest request)
    {
        User user = await _userRepository.Add(request.Adapt<User>());
        
        JwtSecurityToken jwtSecurityToken = GenerateToken(user);
        
        UserAuthenticationResponse response = new ()
        {
            User = user.Adapt<UserDto>(),
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
        };
        
        return response;
    }
    
    private JwtSecurityToken GenerateToken(User user)
    {
        List<Claim> roleClaims = new List<Claim>();

        IEnumerable<Claim> claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
            }
            .Union(roleClaims);

        SymmetricSecurityKey symmetricSecurityKey = new (Encoding.UTF8.GetBytes(_jwtSettings.Key));
        SigningCredentials signingCredentials = new (symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurityToken = new (
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }
}