using WebAPITemplate.Shared.Models.Requests.User;
using WebAPITemplate.Shared.Models.Responses;

namespace WebAPITemplate.API.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAll();
    Task<UserDto> GetById(Guid userId);
    Task<UserDto> GetByUsername(string username);

    Task<UserDto> Add(UserAddRequest user);
    Task<UserDto> Update(UserUpdateRequest user);

    Task<bool> Delete(Guid userId);
}