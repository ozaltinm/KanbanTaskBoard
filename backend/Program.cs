using FluentValidation;
using FluentValidation.AspNetCore;
using KanbanTaskBoard.Api.Application.Services;
using KanbanTaskBoard.Api.Application.Services.Impl;
using KanbanTaskBoard.Api.Application.Validators;
using KanbanTaskBoard.Api.Infrastructure;
using KanbanTaskBoard.Api.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---- CORS (DEV) ----
const string DevCors = "DevCors";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(DevCors, p => p
        .AllowAnyOrigin()  // sorun çözülünce WithOrigins(...) ile daralt
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

// Services
builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("kanban-db"));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskValidator>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ---- Sıra kritik ----
app.UseRouting();
app.UseCors(DevCors);

// app.UseHttpsRedirection(); // container'da HTTP, devde kapalı kalsın

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers().RequireCors(DevCors);
app.MapMethods("{*path}", new[] { "OPTIONS" }, () => Results.Ok())
   .RequireCors(DevCors);
app.MapGet("/", () => Results.Ok(new { status = "ok" }))
   .RequireCors(DevCors);

app.Run();                   // ← top-level statements burada biter

// ← sınıf TANIMI en sonda olacak
public partial class Program { }
