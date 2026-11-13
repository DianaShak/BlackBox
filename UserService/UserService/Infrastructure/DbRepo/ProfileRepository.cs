using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Mappings;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.DbRepo;

public class ProfileRepository : IProfileRepository
{
    private ApplicationDbContext _context;
    
    public ProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var profileModel = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        
        return profileModel == null ? throw new KeyNotFoundException($"Профиль с Id {id} не найден.")
            : ProfileMapper.ToDomain(profileModel);
    }

    public async Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var profileModel = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        
        if (profileModel == null)
        {
            throw new KeyNotFoundException($"Профиль пользователя с Id {userId} не найден.");
        }

        return ProfileMapper.ToDomain(profileModel);
    }

    public async Task AddAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        var profileModel = ProfileMapper.ToModel(profile)!;
        await _context.UserProfiles.AddAsync(profileModel, cancellationToken);
    }

    public async Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        var existingProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.Id == profile.Id, cancellationToken);
        
        if (existingProfile == null)
        {
            throw new KeyNotFoundException($"Профиль с Id {profile.Id} не найден для обновления.");
        }
        
        existingProfile.Bio = profile.Bio;
        existingProfile.BirthDate = profile.BirthDate;
    }

    public async Task DeleteAsync(UserProfile profile, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}