using KanbanTaskBoard.Api.Application.DTOs;
using KanbanTaskBoard.Api.Domain;
using KanbanTaskBoard.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KanbanTaskBoard.Api.Application.Services.Impl;

public class TaskService : ITaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken ct)
    {
        var items = await _db.Tasks.AsNoTracking().OrderBy(t => t.CreatedAt).ToListAsync(ct);
        return items.Select(MapToDto);
    }

    public async Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var item = await _db.Tasks.FindAsync(new object?[] { id }, ct);
        return item is null ? null : MapToDto(item);
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto, CancellationToken ct)
    {
        var entity = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description,
            Status = dto.Status,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        _db.Tasks.Add(entity);
        await _db.SaveChangesAsync(ct);
        return MapToDto(entity);
    }

    public async Task<TaskDto?> UpdateAsync(Guid id, UpdateTaskDto dto, CancellationToken ct)
    {
        var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity is null) return null;

        entity.Title = dto.Title.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description;
        entity.Status = dto.Status;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(ct);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity is null) return false;
        _db.Tasks.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    private static TaskDto MapToDto(TaskItem t) => new(
        t.Id, t.Title, t.Description, t.Status, t.CreatedAt, t.UpdatedAt);
}
