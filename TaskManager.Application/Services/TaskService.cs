using FluentValidation;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IValidator<CreateTaskRequest> _validator;

    public TaskService(ITaskRepository repository, IValidator<CreateTaskRequest> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<CreateTaskResponse> CreateTaskAsync(CreateTaskRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var task = new TaskItem(request.Title, request.Description, request.DueDate);
        await _repository.AddAsync(task);
        return new CreateTaskResponse(task.Id);
    }
}