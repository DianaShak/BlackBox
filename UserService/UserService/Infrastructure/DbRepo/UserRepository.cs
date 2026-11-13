using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Mappings;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.DbRepo;

public class UserRepository : IUserRepository
{
    private ApplicationDbContext _context;
    
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        var userModel = UserMapper.ToModel(user)!;
        await _context.Users.AddAsync(userModel, cancellationToken);
    }

    public async Task DeleteAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var existingModel = await _context.Users
            .Include(u => u.PasswordHistory)
            .FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
        
        if (existingModel == null)
        {
            throw new KeyNotFoundException($"Пользователь с Id {user.Id} не найден для обновления.");
        }
        
        existingModel.Login = user.Login;
        existingModel.Password = user.PasswordHash;
        existingModel.DisplayName = user.DisplayName;
        existingModel.Status =  user.Status;
        existingModel.UserRole = user.UserRole;
        
        foreach (var domainPasswordRecord in user.PasswordHistory)
        {
            bool existsInDb = existingModel.PasswordHistory
                .Any(dbPh => dbPh.Password == domainPasswordRecord.PasswordHash);

            if (!existsInDb)
            {
                existingModel.PasswordHistory.Add(new PasswordRecordModel
                {
                    Id =  Guid.NewGuid(),
                    UserId = user.Id,
                    Password = domainPasswordRecord.PasswordHash,
                    ChangedAt = domainPasswordRecord.ChangedAt
                });
            }
        }
        
        existingModel.PasswordHistory.RemoveAll(dbPh => 
            !user.PasswordHistory.Any(domPh => domPh.PasswordHash == dbPh.Password));
    }
    public async Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
    {
        var userModel = await _context.Users.Include(u => u.PasswordHistory)
            .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
        if (userModel == null)
        {
            throw new KeyNotFoundException($"Пользователь с login {login} не найден.");
        }

        return UserMapper.ToDomain(userModel);
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userModel = await _context.Users.Include(u => u.PasswordHistory)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        return userModel == null ? throw new KeyNotFoundException($"Пользователь с Id {userId} не найден.")
            : UserMapper.ToDomain(userModel);
    }
    
    public async Task<bool> ExistsByLoginAsync(string login, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(u => u.Login == login, cancellationToken);
    }
}