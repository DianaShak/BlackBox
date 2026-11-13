using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Use_Cases.RegisterUser;

public class RegisterUserCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    
    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IProfileRepository profileRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Login) || string.IsNullOrWhiteSpace(command.Password) || string.IsNullOrWhiteSpace(command.Username))
        {
            throw new Exception("Логин, пароль и юзернейм не могут быть пустыми");
        }
        
        var isUserExist = await _userRepository.ExistsByLoginAsync(command.Login, cancellationToken);
        
        if (isUserExist)
        {
            throw new Exception("Пользователь с таким логином уже существует.");
        }
        
        var user = User.Register(command.Login, command.Password, command.Username, _passwordHasher);
        var defaultProfile = UserProfile.CreateDefault(user.Id);
        
        await _userRepository.AddAsync(user, cancellationToken);
        await _profileRepository.AddAsync(defaultProfile, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}