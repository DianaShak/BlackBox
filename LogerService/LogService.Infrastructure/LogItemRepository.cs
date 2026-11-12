using LogerService.Domain.Entities;
using LogerService.Domain.Interfaces;

namespace LogService.Infrastructure;

public class LogItemRepository : ILogItemRepository
{
    private AppDbContext  _context;
    
    public LogItemRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(LogItem item)
    {
        await _context.LogItems.AddAsync(item);
    }

    public Task DeleteAsync(LogItem item)
    {
        return Task.FromResult(_context.LogItems.Remove(item));
    }

    public Task<LogItem?> GetByIdAsync(int id)
    {
        return Task.FromResult(_context.LogItems.Find(id));
    }

    public Task<List<LogItem>> GetByRangeAsync(DateTime start, DateTime end)
    {
        return Task.FromResult(
            _context.LogItems.Where(x => x.OccurredAt >= start && x.OccurredAt <= end
            ).ToList());
    }
}