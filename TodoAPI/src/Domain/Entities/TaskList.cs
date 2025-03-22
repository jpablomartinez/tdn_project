using System.Text.Json.Serialization;

namespace TodoAPI.src.Domain.Entities;

public class TaskList(int id, string title)
{

    public int Id { get; set; } = id;
    public string Title { get; set; } = title;

    public int BoardId { get; set; }

    [JsonIgnore]
    public Board? Board { get; set; }

    public List<TodoTask> Tasks { get; set; } = new();

}