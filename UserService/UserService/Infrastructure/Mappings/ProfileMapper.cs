using UserService.Domain.Entities;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Mappings;

public class ProfileMapper
{
    public static UserProfile? ToDomain(UserProfilesModel? model)
    {
        if (model == null) return null;
        
        return new UserProfile(
            model.Id,
            model.UserId,
            model.Bio,
            model.BirthDate,
            model.CreatedAt
        );
    }
    
    public static UserProfilesModel? ToModel(UserProfile? profile)
    {
        if  (profile == null) return null;

        return new UserProfilesModel
        {
            Id = profile.Id,
            UserId = profile.UserId,
            Bio = profile.Bio,
            BirthDate = profile.BirthDate,
            CreatedAt = profile.CreatedAt
        };
    }
}