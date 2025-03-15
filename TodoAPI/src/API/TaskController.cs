namespace TodoAPI.src.API;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.src.Domain.Entities;

[ApiController]
[Route("api/[controller]")]

public class TaskController : ControllerBase
{
    private readonly TodoTaskDb _todoTaskDbContext;

    public TaskController(TodoTaskDb context)
    {
        _todoTaskDbContext = context;
    }

    private string ParseDescription(string description)
    {
        return string.IsNullOrWhiteSpace(description) ? "No description provided" : description;
    }

    private async Task<TodoTask?> FindById(int id)
    {
        return await _todoTaskDbContext.tasks.FirstOrDefaultAsync(t => t.Id == id);

    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<TodoTask> tasks = await _todoTaskDbContext.tasks.ToListAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        TodoTask? task = await FindById(id);
        return task == null ? NotFound($"Task with id:{id} was not found") : Ok(task);
    }

    [HttpGet("search")]
    /*public IActionResult GetByParams([FromQuery] string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return BadRequest("[ERROR] Title cannot be empty");
        }
        List<TodoTask> _tasks = tasks.FindAll((TodoTask task) => task.Title == title);
        return Ok(_tasks);
    }*/

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoTask request)
    {
        Console.WriteLine(request);
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest("[ERROR] Title cannot be empty");
        }
        string description = ParseDescription(request.Description ?? "");
        TodoTask task = new(request.Id, request.Title, description);
        _todoTaskDbContext.tasks.Add(task);
        await _todoTaskDbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = request.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] TodoTask request, int id)
    {
        TodoTask? task = await FindById(id);
        if (task is null)
        {
            return NotFound($"Task with id:{id} was not found");
        }
        if (string.IsNullOrEmpty(request.Title))
        {
            return BadRequest("[ERROR] Title cannot be empty");
        }
        task.Title = request.Title;
        task.Description = request.Description;
        //task.IsComplete = request.IsComplete;
        await _todoTaskDbContext.SaveChangesAsync();
        return NoContent();
    }


}

