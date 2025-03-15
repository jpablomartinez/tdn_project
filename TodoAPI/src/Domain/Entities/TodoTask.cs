namespace TodoAPI.src.Domain.Entities;

public class TodoTask(int id, string title, string description)
{

    public int Id { get; set; } = id;
    public string Title { get; set; } = title;

    public string? Description { get; set; } = description;

    //public bool? IsComplete { get; set; } = isComplete;

}
