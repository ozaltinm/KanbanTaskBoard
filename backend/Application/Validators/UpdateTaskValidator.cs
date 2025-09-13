using FluentValidation;
using KanbanTaskBoard.Api.Application.DTOs;

namespace KanbanTaskBoard.Api.Application.Validators;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(140);
    }
}

