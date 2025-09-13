using FluentValidation;
using KanbanTaskBoard.Api.Application.DTOs;

namespace KanbanTaskBoard.Api.Application.Validators;
public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(140);
    }
}
