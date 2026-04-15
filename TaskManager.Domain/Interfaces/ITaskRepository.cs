using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task);
    Task<IEnumerable<TaskItem>> SearchAsync(Enums.TaskStatus? status, DateTime? dueDate);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task UpdateAsync(TaskItem task);
}