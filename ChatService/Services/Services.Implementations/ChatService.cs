using AutoMapper;
using Domain.Entities.Entities;
using Services.Abstractions;
using Services.Contracts.Chat;
using Services.Repositories.Abstractions;

namespace Services.Implementations;

/// <summary>
/// Cервис работы с чатами.
/// </summary>
public class ChatService : IChatService
{
    private readonly IMapper _mapper;
    private readonly IChatRepository _chatRepository;
    //private readonly IChatMembreRepository _chatMemberRepository;
    //private readonly IBusControl _busControl;
    //private readonly IUnitOfWork _unitOfWork;

    public ChatService(
        IMapper mapper,
        IChatRepository chatRepository)
        //ILessonRepository lessonRepository,
        //IUnitOfWork unitOfWork,
        //IBusControl busControl)
    {
        _mapper = mapper;
        _chatRepository = chatRepository;
        //_lessonRepository = lessonRepository;
        //_busControl = busControl;
        //_unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Получить чат.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <returns> ДТО курса. </returns>
    public async Task<ChatDto> GetByIdAsync(Guid id)
    {
        var chat = await _chatRepository.GetAsync(id, CancellationToken.None);
        return _mapper.Map<Chat, ChatDto>(chat);
    }

    /// <summary>
    /// Создать чат.
    /// </summary>
    /// <param name="creatingChatDto">ДТО создаваемого чата.</param>
    /// <returns>Идентификатор.</returns>
    public async Task<Guid> CreateAsync(CreatingChatDto creatingChatDto)
    {
        var сhat = _mapper.Map<CreatingChatDto, Chat>(creatingChatDto);
        var createdChat = await _chatRepository.AddAsync(сhat);
        await _chatRepository.SaveChangesAsync();
        /*
        await _busControl.Publish(new MessageDto
        {
            Content = $"Course {createdCourse.Id} with name {createdCourse.Name} is added"
        });
        */
        return createdChat.Id;
    }

    /*
    /// <summary>
    /// Обновить чат и состав сообщений.
    /// Для показа unit of work.
    /// </summary>
    /// <param name="updatingCourseWithLessonsDto"> ДТО редактируемого курса. </param>
    /// <param name="id"> Id </param>
    public async Task UpdatingWithLessonsAsync(int id, UpdatingCourseWithLessonsDto updatingCourseWithLessonsDto)
    {
        //var course = await _unitOfWork.CourseRepository.GetAsync(id, CancellationToken.None);
        var course = await _courseRepository.GetAsync(id, CancellationToken.None);
        if (course == null)
        {
            throw new Exception($"Курс с идентфикатором {id} не найден");
        }

        course.Name = updatingCourseWithLessonsDto.Name;
        course.Price = updatingCourseWithLessonsDto.Price;
        _courseRepository.Update(course);
        await _courseRepository.SaveChangesAsync();
        //_unitOfWork.CourseRepository.Update(course);
        var lessons = _mapper.Map<IEnumerable<AttachingLessonDto>, IEnumerable<Lesson>>(updatingCourseWithLessonsDto.Lessons);
        foreach (var lesson in lessons)
        {
            lesson.CourseId = 100; //Не существует
            await _lessonRepository.AddAsync(lesson);
            //await _unitOfWork.LessonRepository.AddAsync(lesson);
        }

        await _lessonRepository.SaveChangesAsync();
        //await _unitOfWork.SaveChangesAsync();
    }*/

    /// <summary>
    /// Изменить чат.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="updatingChatDto">ДТО редактируемого чата.</param>
    public async Task UpdateAsync(Guid id, UpdatingChatDto updatingChatDto)
    {
        var chat = await _chatRepository.GetAsync(id, CancellationToken.None);
        if (chat == null)
        {
            throw new Exception($"Чат с идентфикатором {id} не найден.");
        }

        chat.Name = updatingChatDto.Name;
        //chat.Price = updatingChatDto.Price;
        _chatRepository.Update(chat);
        await _chatRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить чат.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    public async Task DeleteAsync(Guid id)
    {
        var chat = await _chatRepository.GetAsync(id, CancellationToken.None);
        //chat.Deleted = true;
        await _chatRepository.SaveChangesAsync();
    }

    /*
    /// <summary>
    /// Получить постраничный список.
    /// </summary>
    /// <param name="filterDto">ДТО фильтра.</param>
    /// <returns>Список чатов.</returns>
    public async Task<ICollection<ChatDto>> GetPagedAsync(ChatFilterDto filterDto)
    {
        ICollection<Chat> entities = await _chatRepository.GetPagedAsync(filterDto);
        return _mapper.Map<ICollection<Chat>, ICollection<ChatDto>>(entities);
    }

    public async Task<IEnumerable<ChatInfoDto>> GetChatInfosAsync(string fieldsToSelect)
    {
        var entities = await _chatRepository.GetChatInfosAsync(fieldsToSelect);
        return _mapper.Map<IEnumerable<ChatInfo>, IEnumerable<ChatInfoDto>>(entities);
    }*/

    public Task UpdatingWithMembersAsync(Guid id, UpdatingChatWithMembersDto updatingChatWithMembersDto)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<ChatDto>> GetPagedAsync(ChatFilterDto filterDto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ChatInfoDto>> GetChatInfosAsync(string fieldsToSelect)
    {
        throw new NotImplementedException();
    }
}
