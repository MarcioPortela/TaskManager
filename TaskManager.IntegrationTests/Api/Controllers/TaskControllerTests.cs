using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TaskManager.Application.DTOs;
using TaskManager.IntegrationTests.Infrastructure;

namespace TaskManager.IntegrationTests.Controllers;

public class TasksControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TasksControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_ShouldReturn201Created_WhenRequestIsValid()
    {
        var request = new CreateTaskRequest
        {
            Title = "Teste de Integração",
            Description = "Utilizar teste de integração",
            DueDate = DateTime.UtcNow.AddDays(2)
        };

        var response = await _client.PostAsJsonAsync("/api/v1/tasks", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateTaskResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Post_ShouldReturn400BadRequest_WhenTitleIsEmpty()
    {
        var request = new CreateTaskRequest
        {
            Title = "",
            Description = "Descrição teste"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/tasks", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be("Erro de Validação");
        problemDetails.Errors.Should().ContainKey("Title");
    }

    [Fact]
    public async Task Get_ShouldReturn200Ok_AndListOfTasks()
    {
        var firstRequest = new CreateTaskRequest { Title = "Tarefa 1", Status = (Domain.Enums.TaskStatus)1 };
        await _client.PostAsJsonAsync("/api/v1/tasks", firstRequest);
        var secondRequest = new CreateTaskRequest { Title = "Tarefa 2", Status = (Domain.Enums.TaskStatus)1 };
        await _client.PostAsJsonAsync("/api/v1/tasks", secondRequest);


        var response = await _client.GetAsync("/api/v1/tasks");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await response.Content.ReadFromJsonAsync<IEnumerable<GetTasksResponse>>();
        tasks.Should().NotBeNullOrEmpty();
        tasks.Should().Contain(t => t.Title == "Tarefa 1");
        tasks.Should().Contain(t => t.Title == "Tarefa 2");
    }
}

public class ValidationProblemDetails
{
    public string Title { get; set; } = string.Empty;
    public Dictionary<string, string[]> Errors { get; set; } = new();
}