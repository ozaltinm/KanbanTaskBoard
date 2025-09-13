using KanbanTaskBoard.Api.Domain;

namespace KanbanTaskBoard.Api.Application.DTOs;

public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    KanbanTaskStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);

