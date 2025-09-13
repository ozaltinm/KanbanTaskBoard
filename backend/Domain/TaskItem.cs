using System.ComponentModel.DataAnnotations;

namespace KanbanTaskBoard.Api.Domain;

public class TaskItem
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public KanbanTaskStatus Status { get; set; } = KanbanTaskStatus.ToDo;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}

 