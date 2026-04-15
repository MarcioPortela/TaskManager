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
        // ==========================================
        // TODO: LOG TRANSACIONAL (INÍCIO)
        // Registrar tentativa de criação de tarefa.
        // ==========================================

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var task = new TaskItem(request.Title, request.Description, request.DueDate);
        await _repository.AddAsync(task);

        // ==========================================
        // TODO: LOG TRANSACIONAL (SUCESSO)
        // Registrar que o fluxo de criação terminou com sucesso.
        // ==========================================

        return new CreateTaskResponse(task.Id);
    }

    public async Task<IEnumerable<GetTasksResponse>> GetAllAsync(Domain.Enums.TaskStatus? status, DateTime? dueDate)
    {
        // ==========================================
        // TODO: LOG TRANSACIONAL (INÍCIO)
        // Registrar tentativa de pesquisa de tarefas.
        // ==========================================

        var tasks = await _repository.SearchAsync(status, dueDate);

        // ==========================================
        // TODO: LOG TRANSACIONAL (SUCESSO)
        // Registrar que o fluxo de pesquisa terminou com sucesso.
        // ==========================================

        return tasks.Select(t => new GetTasksResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            Status = t.Status.ToString()
        });

    }

    public async Task UpdateTaskAsync(Guid id, UpdateTaskRequest request)
    {
        // ==========================================
        // TODO: LOG TRANSACIONAL (INÍCIO)
        // Registrar tentativa de atualização de tarefa.
        // ==========================================

        var task = await _repository.GetByIdAsync(id);

        if (task == null)
            throw new KeyNotFoundException("Tarefa não encontrada.");

        task.Update(request.Title, request.Description, request.DueDate, request.Status);

        await _repository.UpdateAsync(task);

        // ==========================================
        // TODO: LOG TRANSACIONAL (SUCESSO)
        // Registrar que o fluxo de atualização terminou com sucesso.
        // ==========================================
    }

    public async Task DeleteTaskAsync(Guid id)
    {
        // ==========================================
        // TODO: LOG TRANSACIONAL (INÍCIO)
        // Registrar tentativa de exclusão de tarefa.
        // ==========================================

        var task = await _repository.GetByIdAsync(id);

        if (task == null)
            throw new KeyNotFoundException("Tarefa não encontrada para exclusão.");

        await _repository.DeleteAsync(task);

        // ==========================================
        // TODO: LOG TRANSACIONAL (SUCESSO)
        // Registrar que o fluxo de exclusão terminou com sucesso.
        // ==========================================
    }
}