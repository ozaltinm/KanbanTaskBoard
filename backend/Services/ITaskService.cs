using KanbanTaskBoard.Api.Application.DTOs;

namespace KanbanTaskBoard.Api.Application.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken ct);
    Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<TaskDto> CreateAsync(CreateTaskDto dto, CancellationToken ct);
    Task<TaskDto?> UpdateAsync(Guid id, UpdateTaskDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}

