using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.src.Domain.Entities;

namespace TodoAPI.src.API;

[ApiController]
[Route("api/[controller]")]
public class BoardController : ControllerBase
{
    private readonly TodoTaskDb _todoTaskDbContext;

    public BoardController(TodoTaskDb context)
    {
        _todoTaskDbContext = context;
    }

    /// <summary>
    /// Asynchronously retrieves all boards from the database, including their associated task lists and tasks.
    /// </summary>
    /// <returns>An IActionResult containing a list of all boards with their task lists and tasks.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllBoards()
    {
        List<Board> boards = await _todoTaskDbContext.Boards
        .Include(b => b.TaskLists)
        .ThenInclude(tl => tl.Tasks)
        .ToListAsync();
        return Ok(new { data = boards });
    }

    /// <summary>
    /// Asynchronously retrieves a board by its ID, including its task lists and tasks.
    /// </summary>
    /// <param name="id">The ID of the board to retrieve.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the board if found, or a NotFound result with a message if not found.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBoardById(int id)
    {
        Board? board = await _todoTaskDbContext.Boards
        .Include(b => b.TaskLists)
        .ThenInclude(tl => tl.Tasks)
        .FirstOrDefaultAsync(b => b.Id == id);
        if (board == null)
        {
            return NotFound(new { Message = "Board not found", Id = id });
        }
        return Ok(new { data = board });
    }

    /// <summary>
    /// Handles the creation of a new board by adding it to the database.
    /// </summary>
    /// <param name="board">The board object to be created, received from the request body.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an 
    /// <see cref="IActionResult"/> that returns a 201 Created response with the location of the new board.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] Board board)
    {
        _todoTaskDbContext.Boards.Add(board);
        await _todoTaskDbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBoardById), new { id = board.Id }, board);
    }
}