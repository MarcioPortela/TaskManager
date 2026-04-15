using TaskManager.Application.DTOs;

namespace TaskManager.Application.Services
{
    public interface ITaskService
    {
        Task<CreateTaskResponse> CreateTaskAsync(CreateTaskRequest request);
    }
}