using WebAPITemplate.Shared.Models.Entities;

namespace WebAPITemplate.API.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(Guid userId);
    Task<User> GetByUsername(string username);

    Task<User> Add(User user);

    Task<User> Update(User user);
    Task<bool> Delete(Guid userId);
}