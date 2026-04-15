using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Tests.Entities
{
    public class TaskItemTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateTaskItem()
        {
            string title = "Tarefa teste";
            string description = "Descrição teste";
            DateTime dueDate = DateTime.Now.AddDays(1);
            Enums.TaskStatus status = (Enums.TaskStatus)1;

            var task = new TaskItem(title, status, description, dueDate);

            Assert.NotEqual(Guid.Empty, task.Id);
            Assert.Equal(title, task.Title);
            Assert.Equal(description, task.Description);
            Assert.Equal(dueDate, task.DueDate);
            Assert.Equal(status, task.Status);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidTitle_ShouldThrowArgumentException(string invalidTitle)
        {
            Enums.TaskStatus status = (Enums.TaskStatus)1;

            var exception = Assert.Throws<ArgumentException>(() =>
                new TaskItem(invalidTitle, status));

            Assert.Equal("title", exception.ParamName);
            Assert.Contains("O título é obrigatório.", exception.Message);
        }

        [Fact]
        public void Update_WithValidParameters_ShouldUpdateProperties()
        {
            var task = new TaskItem("Titulo original", 0, "Descrição original", DateTime.Now);

            string newTitle = "Titulo atualizado";
            string newDescription = "Descrição atualizada";
            DateTime newDueDate = DateTime.Now.AddDays(5);
            Enums.TaskStatus newStatus = (Enums.TaskStatus)1;

            task.Update(newTitle, newDescription, newDueDate, newStatus);

            Assert.Equal(newTitle, task.Title);
            Assert.Equal(newDescription, task.Description);
            Assert.Equal(newDueDate, task.DueDate);
            Assert.Equal(newStatus, task.Status);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Update_WithInvalidTitle_ShouldThrowArgumentException(string invalidTitle)
        {
            var task = new TaskItem("Primeiro titulo", 0);

            var exception = Assert.Throws<ArgumentException>(() =>
                task.Update(invalidTitle, "Nova descrição", DateTime.Now, (Enums.TaskStatus)1));

            Assert.Equal("title", exception.ParamName);
            Assert.Contains("O título é obrigatório.", exception.Message);
        }
    }
}