using KanbanTaskBoard.Api.Domain;

namespace KanbanTaskBoard.Api.Application.DTOs;

public record UpdateTaskDto(
    string Title,
    string? Description,
    KanbanTaskStatus Status
);

