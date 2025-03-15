namespace TodoAPI.src.Domain.Entities;

public class TodoTask(string title, string description)
{

    public string? Id { get; private set; }
    public string Title { get; private set; } = title;

    public string? Description { get; private set; } = description;
}
