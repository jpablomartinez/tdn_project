using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.src.Domain.Entities;

namespace TodoAPI.src.API;

[ApiController]
[Route("api/[controller]")]

public class TaskListController : ControllerBase
{

    private readonly TodoTaskDb _todoTaskDbContext;

    public TaskListController(TodoTaskDb context)
    {
        _todoTaskDbContext = context;
    }

    /// <summary>
    /// Asynchronously retrieves all task lists along with their associated tasks from the database.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the list of task lists.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllListByBoard()
    {
        List<TaskList> taskLists = await _todoTaskDbContext.TaskLists
        .Include(t => t.Tasks)
        .ToListAsync();
        return Ok(taskLists);
    }

    /// <summary>
    /// Retrieves a TaskList by its identifier, including its associated tasks.
    /// </summary>
    /// <param name="id">The identifier of the TaskList to retrieve.</param>
    /// <returns>
    /// An IActionResult containing the TaskList if found; otherwise, a NotFound result with an error message.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetListById(int id)
    {
        TaskList? taskList = await _todoTaskDbContext.TaskLists
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (taskList == null)
        {
            return NotFound(new { Message = "TaskList not found", Id = id });
        }
        return Ok(taskList);
    }

    /// <summary>
    /// Handles the HTTP POST request to create a new task list.
    /// </summary>
    /// <param name="taskList">The task list object to be created, received from the request body.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult 
    /// that indicates the result of the action, including a 201 Created response with the location of the new resource.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateList([FromBody] TaskList taskList)
    {
        _todoTaskDbContext.TaskLists.Add(taskList);
        await _todoTaskDbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetListById), new { id = taskList.Id }, taskList);
    }


}