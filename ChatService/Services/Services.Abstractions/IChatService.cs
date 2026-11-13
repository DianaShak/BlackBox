using Services.Contracts.Chat;

namespace Services.Abstractions;

/// <summary>
/// Интерфейс сервиса работы с чатами.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Получить чат.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>ДТО курса.</returns>
    Task<ChatDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Создать чат.
    /// </summary>
    /// <param name="creatingChatDto">ДТО создаваемого чата.</param>
    Task<Guid> CreateAsync(CreatingChatDto creatingChatDto);

    /// <summary>
    /// Обновить чат и состав учасников.
    /// Для показа unit of work.
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="updatingChatWithMembersDto"></param>
    Task UpdatingWithMembersAsync(Guid id, UpdatingChatWithMembersDto updatingChatWithMembersDto);

    /// <summary>
    /// Изменить чат.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="updatingChatDto">ДТО редактируемого чата.</param>
    Task UpdateAsync(Guid id, UpdatingChatDto updatingChatDto);

    /// <summary>
    /// Удалить курс.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Получить постраничный список.
    /// </summary>
    /// <param name="filterDto">ДТО фильтра.</param>
    /// <returns>Список чатов.</returns>
    Task<ICollection<ChatDto>> GetPagedAsync(ChatFilterDto filterDto);

    Task<IEnumerable<ChatInfoDto>> GetChatInfosAsync(string fieldsToSelect);
}
