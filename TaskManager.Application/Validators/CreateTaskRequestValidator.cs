using FluentValidation;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Validators;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título da tarefa é obrigatório e não pode estar em branco.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("O status da tarefa deve ser um valor válido (0=Pending, 1=InProgress, 2=Completed).");
    }
}