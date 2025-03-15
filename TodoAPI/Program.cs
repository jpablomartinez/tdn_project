using Microsoft.EntityFrameworkCore;
using TodoAPI.src.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoTaskDb>(opt => opt.UseInMemoryDatabase("tasks"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
var app = builder.Build();

//(app.MapGet("/todo/task", () => taskController.GetAll());

app.MapControllers();

app.Run();
