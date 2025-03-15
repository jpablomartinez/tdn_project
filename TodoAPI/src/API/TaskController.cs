namespace TodoAPI.src.API;

using Microsoft.AspNetCore.Mvc;
using TodoAPI.src.Domain.Entities;

[ApiController]
[Route("api/[controller]")]

public class TaskController : ControllerBase
{
    List<TodoTask> tasks = [new("Tarea 1", "Ejemplo de tarea 1"), new("Tarea 2", "Ejemplo de tarea 2"), new("Tarea 3", "Ejemplo de tarea 3")];

    [HttpGet]
    public IActionResult GetAll()
    {
        //List<TodoTask> tasks = [new("Tarea 1", "Ejemplo de tarea 1"), new("Tarea 2", "Ejemplo de tarea 2"), new("Tarea 3", "Ejemplo de tarea 3")];
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        if (id > tasks.Count - 1 || id < 0)
        {
            return NotFound($"Task with id:{id} was not found");
        }
        return Ok(tasks[id]);
    }

    [HttpGet("search")]
    public IActionResult GetByTitle([FromQuery] string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return BadRequest("[ERROR] Title cannot be empty");
        }
        List<TodoTask> _tasks = tasks.FindAll((TodoTask task) => task.Title == title);
        return Ok(_tasks);
    }

    [HttpPost]
    public IActionResult Create([FromBody] TodoTask request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest("[ERROR] Title cannot be empty");
        }
        string description = string.IsNullOrWhiteSpace(request.Description) ? "No description provided" : request.Description;
        TodoTask task = new(request.Title, description);
        tasks.Add(task);
        return CreatedAtAction(nameof(GetById), new { id = tasks.Count - 1 }, task);
    }

}

