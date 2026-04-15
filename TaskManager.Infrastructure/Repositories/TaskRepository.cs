using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskManagerDbContext _context;

    public TaskRepository(TaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskItem task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskItem>> SearchAsync(Domain.Enums.TaskStatus? status, DateTime? dueDate)
    {
        IQueryable<TaskItem> query = _context.Tasks;

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (dueDate.HasValue)
        {
            query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate.Value.Date);
        }

        return await query.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }
}