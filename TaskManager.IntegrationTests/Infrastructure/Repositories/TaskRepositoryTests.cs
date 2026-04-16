using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.IntegrationTests.Infrastructure.Repositories
{
    public class TaskRepositoryTests
    {
        private readonly TaskManagerDbContext _context;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
                .UseInMemoryDatabase(databaseName: $"TaskManagerDb_Test_{Guid.NewGuid()}")
                .Options;

            _context = new TaskManagerDbContext(options);
            _repository = new TaskRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveTaskToDatabase()
        {
            var task = new TaskItem("Tarefa 1", Domain.Enums.TaskStatus.Pending, "Descrição", DateTime.Now);

            await _repository.AddAsync(task);

            var savedTask = await _context.Tasks.FindAsync(task.Id);

            savedTask.Should().NotBeNull();
            savedTask!.Title.Should().Be("Tarefa 1");
            savedTask.Status.Should().Be(Domain.Enums.TaskStatus.Pending);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTask_WhenTaskExists()
        {
            var task = new TaskItem("Tarefa 2", Domain.Enums.TaskStatus.InProgress);
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(task.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(task.Id);
        }

        [Fact]
        public async Task SearchAsync_ShouldFilterByStatusAndDueDate()
        {
            var date1 = DateTime.UtcNow.AddDays(1);
            var date2 = DateTime.UtcNow.AddDays(5);

            var task1 = new TaskItem("Tarefa 1", Domain.Enums.TaskStatus.Completed, dueDate: date1);
            var task2 = new TaskItem("Tarefa 2", Domain.Enums.TaskStatus.Pending, dueDate: date1);
            var task3 = new TaskItem("Tarefa 3", Domain.Enums.TaskStatus.Pending, dueDate: date2);

            _context.Tasks.AddRange(task1, task2, task3);
            await _context.SaveChangesAsync();

            var pendingTasks = await _repository.SearchAsync(Domain.Enums.TaskStatus.Pending, null);

            pendingTasks.Should().HaveCount(2);
            pendingTasks.Should().Contain(t => t.Id == task2.Id);
            pendingTasks.Should().Contain(t => t.Id == task3.Id);

            var specificTasks = await _repository.SearchAsync(Domain.Enums.TaskStatus.Completed, date1);

            specificTasks.Should().HaveCount(1);
            specificTasks.First().Id.Should().Be(task1.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTaskFromDatabase()
        {
            var task = new TaskItem("Tarefa 4", Domain.Enums.TaskStatus.InProgress);
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(task);

            var deletedTask = await _context.Tasks.FindAsync(task.Id);
            deletedTask.Should().BeNull();
        }
    }
}
