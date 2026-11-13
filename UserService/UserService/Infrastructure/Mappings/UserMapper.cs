using UserService.Domain.Entities;
using UserService.Domain.Value_Objects;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Mappings;

public static class UserMapper
{
    public static User? ToDomain(UsersModel? model)
    {
        if (model == null) return null;
        var domainPasswordHistory = model.PasswordHistory
            .Select(ph => new PasswordRecord(ph.Password, ph.ChangedAt))
            .ToList();
        
        return new User(
            model.Id,
            model.Login,
            model.Password,
            model.DisplayName,
            model.Status,
            (UserRole)model.UserRole,
            domainPasswordHistory
            );
    }

    public static UsersModel? ToModel(User? user)
    {
        if (user == null) return null;
        var domainPasswordHistory = user.PasswordHistory
            .Select(ph => new PasswordRecord(ph.PasswordHash, ph.ChangedAt));

        var userModel = new UsersModel
        {
            Id = user.Id,
            Login = user.Login,
            Password = user.PasswordHash,
            DisplayName = user.DisplayName,
            Status = user.Status,
            UserRole = user.UserRole
        };
        userModel.PasswordHistory = user.PasswordHistory
            .Select(ph => new PasswordRecordModel
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Password = ph.PasswordHash,
                ChangedAt = ph.ChangedAt
            })
            .ToList();

        return userModel;
    }
}