using Domain.Entities.Entities;
using Infrastructure.EntityFramework;
using Services.Repositories.Abstractions;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Chat;

namespace Infrastructure.Repositories.Implementations;

public class ChatRepository : Repository<Chat, Guid>, IChatRepository
{
    public ChatRepository(ChatServiceContext context) : base(context)
    {
    }

    /// <summary>
    /// Получить сущность по ID.
    /// </summary>
    /// <param name="id"> Id сущности. </param>
    /// <param name="cancellationToken"></param>
    /// <returns> Курс. </returns>
    public override async Task<Chat> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var query = Context.Set<Chat>().AsQueryable();
        return await query.SingleOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Получить постраничный список.
    /// </summary>
    /// <param name="filterDto"> ДТО фильтра. </param>
    /// <returns> Список курсов. </returns>
    public async Task<List<Chat>> GetPagedAsync(ChatFilterDto filterDto)
    {
        var query = GetAll()
            .Where(c => c.Name == "Example");
        //.Include(c => c.Lessons).AsQueryable();
        if (!string.IsNullOrWhiteSpace(filterDto.Name))
        {
            query = query.Where(c => c.Name == filterDto.Name);
        }

        if (!string.IsNullOrWhiteSpace(filterDto.Name))
        {
            query = query.Where(c => c.Name == filterDto.Name);
        }

        query = query
            .Skip((filterDto.Page - 1) * filterDto.ItemsPerPage)
            .Take(filterDto.ItemsPerPage);

        return query.ToList();
    }

    public async Task<List<ChatInfo>> GetCourseInfosAsync(string fieldsToSelect)
    {
        var listOfFieldsToSelect = fieldsToSelect.Split(",");
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendJoin(",", listOfFieldsToSelect.Select(s => $"\"{s}\""));
        PropertyInfo[] properties = typeof(ChatInfo).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (!listOfFieldsToSelect.Contains(property.Name))
            {
                stringBuilder.Append($", NULL as \"{property.Name}\"");
            }
        }
        var courseInfos = await Context.Database.SqlQueryRaw<ChatInfo>($"select {stringBuilder.ToString()} from \"Chats\"").ToListAsync();

        return courseInfos;
    }
}
