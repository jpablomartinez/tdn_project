namespace TodoAPI.src.API;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
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

    /// <summary>
    /// Parses the given description string and returns a default message if it is null or whitespace.
    /// </summary>
    /// <param name="description">The description string to parse.</param>
    /// <returns>The original description if it is not null or whitespace; otherwise, a default message.</returns>
    private static string ParseDescription(string description)
    {
        return string.IsNullOrWhiteSpace(description) ? "No description provided" : description;
    }

    /// <summary>
    /// Parses the difficulty level and ensures it is within the range of 1 to 5.
    /// </summary>
    /// <param name="difficulty">The input difficulty level.</param>
    /// <returns>
    /// Returns 1 if the input difficulty is less than or equal to 0,
    /// returns 5 if the input difficulty is greater than 5,
    /// otherwise returns the input difficulty.
    /// </returns>
    private static int ParseDifficulty(int difficulty)
    {
        return difficulty <= 0 ? 1 : difficulty > 5 ? 5 : difficulty;
    }

    /// <summary>
    /// Asynchronously finds a TodoTask by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the TodoTask to find.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the TodoTask if found; otherwise, null.
    /// </returns>
    private async Task<TodoTask?> FindById(int id)
    {
        return await _todoTaskDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);

    }

    /// <summary>
    /// Asynchronously retrieves all TodoTask entities from the database and returns them as an HTTP response.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult with the list of TodoTask entities.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<TodoTask> tasks = await _todoTaskDbContext.Tasks.ToListAsync();
        return Ok(new { data = tasks });
    }

    /// <summary>
    /// Retrieves a TodoTask by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the TodoTask to retrieve.</param>
    /// <returns>
    /// An IActionResult containing the TodoTask if found, or a NotFound result
    /// if the task with the specified identifier does not exist.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        TodoTask? task = await FindById(id);
        return task == null ? NotFound(new { Message = "Task not found!", Id = id }) : Ok(new { data = task });
    }

    /// <summary>
    /// Handles the creation of a new TodoTask.
    /// </summary>
    /// <param name="request">The TodoTask object containing the task details from the request body.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult that indicates the result of the action.</returns>
    /// <remarks>
    /// Returns a BadRequest if the task title is empty. Otherwise, it adds the task to the database and returns a CreatedAtAction result.
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoTask request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            Log.Error("Title cannot be empty");
            return BadRequest("[ERROR] Title cannot be empty");
        }
        string description = ParseDescription(request.Description ?? "");
        int difficulty = ParseDifficulty(request.Difficulty ?? 0);
        TodoTask task = new(request.Id, request.Title, description, false, DateTime.Now, request.CompleteAt, request.Deadline, difficulty, request.TaskListId);
        _todoTaskDbContext.Tasks.Add(task);
        await _todoTaskDbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = request.Id }, task);
    }

    /// <summary>
    /// Updates an existing TodoTask with new data provided in the request body.
    /// </summary>
    /// <param name="request">The TodoTask object containing updated data.</param>
    /// <param name="id">The ID of the task to be updated.</param>
    /// <returns>
    /// Returns a NotFound result if the task with the specified ID does not exist.
    /// Returns a BadRequest result if the Title in the request is null or empty.
    /// Returns a NoContent result upon successful update.
    /// </returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] TodoTask request, int id)
    {
        TodoTask? task = await FindById(id);
        if (task is null)
        {
            return NotFound($"Task with id:{id} was not found!");
        }
        if (string.IsNullOrEmpty(request.Title))
        {
            return BadRequest("[ERROR] Title cannot be empty");
        }
        task.Title = request.Title;
        task.Description = request.Description;
        task.IsComplete = request.IsComplete;
        task.CompleteAt = request.CompleteAt;
        task.Deadline = request.Deadline;
        task.Difficulty = request.Difficulty;
        try
        {
            await _todoTaskDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating the task with id:{id}", id);
            return BadRequest("[ERROR] An error occurred while updating the task");
        }
        return NoContent();
    }


}

