using Domain.Entities.Entities;

namespace Services.Repositories.Abstractions;

/// <summary>
/// Репозиторий работы с чатами.
/// </summary>
public interface IChatRepository : IRepository<Chat, Guid>
{

}
