using Serilog;
using Microsoft.EntityFrameworkCore;
using TodoAPI.src.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddDbContext<TodoTaskDb>(opt => opt.UseInMemoryDatabase("tasks"));
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

app.Run();
