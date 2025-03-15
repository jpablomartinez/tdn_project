using TodoAPI.src.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();



//(app.MapGet("/todo/task", () => taskController.GetAll());

app.MapControllers();

app.Run();
