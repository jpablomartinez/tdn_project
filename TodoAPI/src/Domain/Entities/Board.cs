namespace TodoAPI.src.Domain.Entities;

public class Board(int id, string title)
{

    public int Id { get; set; } = id;

    public string Title { get; set; } = title;

    public List<TaskList> TaskLists { get; set; } = new();

}