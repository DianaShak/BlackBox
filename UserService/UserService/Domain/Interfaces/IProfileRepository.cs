using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IProfileRepository
{
    Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    Task AddAsync(UserProfile profile, CancellationToken cancellationToken);

    Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken);
    
    Task DeleteAsync(UserProfile profile, CancellationToken cancellationToken);
}