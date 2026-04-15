using FluentValidation;
using FluentValidation.Results;
using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Tests.Application.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock;
        private readonly Mock<IValidator<CreateTaskRequest>> _validatorMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _validatorMock = new Mock<IValidator<CreateTaskRequest>>();
            _taskService = new TaskService(_repositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_WithValidRequest_ShouldReturnResponse()
        {
            var request = new CreateTaskRequest
            {
                Title = "Tarefa teste",
                Description = "Descrição",
                Status = Domain.Enums.TaskStatus.Pending,
                DueDate = DateTime.Now.AddDays(1)
            };

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
                .Returns(Task.CompletedTask);

            var response = await _taskService.CreateTaskAsync(request);

            Assert.NotNull(response);
            Assert.NotEqual(Guid.Empty, response.Id);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task CreateTaskAsync_WithInvalidRequest_ShouldThrowValidationException()
        {
            var request = new CreateTaskRequest { Title = "" };
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Title", "O título é obrigatório") };

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(validationFailures));

            await Assert.ThrowsAsync<ValidationException>(() => _taskService.CreateTaskAsync(request));
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedTasks()
        {
            var tasks = new List<TaskItem>
            {
                new TaskItem("Tarefa 1", Domain.Enums.TaskStatus.Pending, "Descrição 1", DateTime.Now),
                new TaskItem("Tarefa 2", Domain.Enums.TaskStatus.Completed, "Descrição 2", DateTime.Now.AddDays(-1))
            };

            _repositoryMock.Setup(r => r.SearchAsync(null, null))
                .ReturnsAsync(tasks);

            var result = await _taskService.GetAllAsync(null, null);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Tarefa 1", result.First().Title);
        }

        [Fact]
        public async Task UpdateTaskAsync_ExistingTask_ShouldUpdateSuccessfully()
        {
            var taskId = Guid.NewGuid();
            var existingTask = new TaskItem("Titulo original", Domain.Enums.TaskStatus.Pending);
            var request = new UpdateTaskRequest
            {
                Title = "Novo titulo",
                Description = "Nova descrição",
                Status = Domain.Enums.TaskStatus.Completed,
                DueDate = DateTime.Now.AddDays(2)
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync(existingTask);

            await _taskService.UpdateTaskAsync(taskId, request);

            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<TaskItem>(t => t.Title == "Novo titulo")), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_NonExistingTask_ShouldThrowKeyNotFoundException()
        {
            var taskId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync((TaskItem?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _taskService.UpdateTaskAsync(taskId, new UpdateTaskRequest { Title = "Titulo" }));
        }

        [Fact]
        public async Task DeleteTaskAsync_ExistingTask_ShouldDeleteSuccessfully()
        {
            var taskId = Guid.NewGuid();
            var existingTask = new TaskItem("Tarefa 1", Domain.Enums.TaskStatus.Pending);

            _repositoryMock.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync(existingTask);

            await _taskService.DeleteTaskAsync(taskId);

            _repositoryMock.Verify(r => r.DeleteAsync(existingTask), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_NonExistingTask_ShouldThrowKeyNotFoundException()
        {
            var taskId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(taskId))
                .ReturnsAsync((TaskItem?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.DeleteTaskAsync(taskId));
        }
    }
}