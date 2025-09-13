using KanbanTaskBoard.Api.Domain;

namespace KanbanTaskBoard.Api.Application.DTOs;
public record CreateTaskDto(
    string Title,
    string? Description,
    KanbanTaskStatus Status = KanbanTaskStatus.ToDo
);

