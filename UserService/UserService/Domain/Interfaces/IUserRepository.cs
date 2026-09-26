using UserService.Domain.Entities;

namespace UserService.Domain;

public interface IUserRepository
{
    Task SaveUser(User user);
    Task DeleteUser(User user);
    Task<User> GetUserByName(string username);
    Task<User> GetUserById(Guid userId);
}