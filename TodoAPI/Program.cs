using Serilog;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddDbContext<TodoTaskDb>(opt => opt.UseInMemoryDatabase("tasks"));
builder.Services.AddControllers();
var app = builder.Build();


// Configure middleware for error handling
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        if (!context.Response.HasStarted)
        {
            Log.Error($"Global exception caught: {ex.Message} \n{ex.StackTrace}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = "An unexpected error occurred. Please try again later.",
                Error = ex.Message,
            });
        }
    }
});


app.MapControllers();

app.Run();
