using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Cria uma nova tarefa no sistema.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /api/v1/tasks
    ///     {
    ///        "title": "Apontar horas utilizadas",
    ///        "description": "Colocar as horas que foram utilizadas na tarefa no board do Azure DevOps",
    ///        "dueDate": "2026-04-30T23:59:59Z"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Objeto com os dados da tarefa a ser criada.</param>
    /// <returns>Retorna a tarefa criada com seu ID.</returns>
    /// <response code="201">A tarefa foi criada com sucesso.</response>
    /// <response code="400">Os dados enviados são inválidos (ex: Título vazio).</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateTaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var response = await _taskService.CreateTaskAsync(request);

        return Created($"/api/v1/tasks/{response.Id}", response);
    }
}