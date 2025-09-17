using FluentValidation;
using FluentValidation.AspNetCore;
using KanbanTaskBoard.Api.Application.Services;
using KanbanTaskBoard.Api.Application.Services.Impl;
using KanbanTaskBoard.Api.Application.Validators;
using KanbanTaskBoard.Api.Infrastructure;
using KanbanTaskBoard.Api.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", p => p
        .WithOrigins("http://localhost:5173") // frontend adresin
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

// Services
//builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("kanban-db"));
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
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

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers().RequireCors("CorsPolicy");
app.MapMethods("{*path}", new[] { "OPTIONS" }, () => Results.Ok())
   .RequireCors("CorsPolicy");
app.MapGet("/", () => Results.Ok(new { status = "ok" }))
   .RequireCors("CorsPolicy");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();                  

// Make the implicit Program class public so test projects can access it
public partial class Program { }
