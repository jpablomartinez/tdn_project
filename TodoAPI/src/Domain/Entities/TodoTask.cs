namespace TodoAPI.src.Domain.Entities;

public class TodoTask(int id, string title, string? description, bool? isComplete, DateTime? createdAt, DateTime? completeAt, DateTime? deadline)
{

    public int Id { get; set; } = id;
    public string Title { get; set; } = title;

    public string? Description { get; set; } = description;

    public bool? IsComplete { get; set; } = isComplete;

    public DateTime? CreatedAt { get; set; } = createdAt;

    public DateTime? CompleteAt { get; set; } = completeAt;

    public DateTime? Deadline { get; set; } = deadline;
}
