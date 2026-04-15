using TaskManager.Application.DTOs;
using TaskManager.Application.Validators;

namespace TaskManager.Tests.Application.Validators
{
    public class CreateTaskRequestValidatorTests
    {
        private readonly CreateTaskRequestValidator _validator;

        public CreateTaskRequestValidatorTests()
        {
            _validator = new CreateTaskRequestValidator();
        }

        [Fact]
        public void Validate_WithValidRequest_ShouldNotHaveValidationErrors()
        {
            var request = new CreateTaskRequest
            {
                Title = "Título 1",
                Description = "Descrição",
                Status = Domain.Enums.TaskStatus.Pending,
                DueDate = DateTime.Now.AddDays(1)
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_WithInvalidTitle_ShouldHaveValidationError(string invalidTitle)
        {
            var request = new CreateTaskRequest
            {
                Title = invalidTitle,
                Status = Domain.Enums.TaskStatus.Pending
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e =>
                e.PropertyName == "Title" &&
                e.ErrorMessage == "O título da tarefa é obrigatório e não pode estar em branco.");
        }

        [Fact]
        public void Validate_WithInvalidStatus_ShouldHaveValidationError()
        {
            var request = new CreateTaskRequest
            {
                Title = "Título 2",
                Status = (Domain.Enums.TaskStatus)100
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e =>
                e.PropertyName == "Status" &&
                e.ErrorMessage == "O status da tarefa deve ser um valor válido (0=Pending, 1=InProgress, 2=Completed).");
        }
    }
}