using LogerService.Domain.Entities;

namespace LogerService.Domain.Interfaces;

public interface ILogItemRepository
{
    Task AddAsync(LogItem item);
    Task DeleteAsync(LogItem item);
    Task<LogItem?> GetByIdAsync(int id);
    Task<List<LogItem>> GetByRangeAsync(DateTime start, DateTime end);
}