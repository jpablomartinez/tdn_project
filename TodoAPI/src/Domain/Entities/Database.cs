using Microsoft.EntityFrameworkCore;
using TodoAPI.src.Domain.Entities;

public class TodoTaskDb : DbContext
{
    public TodoTaskDb(DbContextOptions<TodoTaskDb> options)
        : base(options) { }

    public DbSet<Board> Boards => Set<Board>();
    public DbSet<TaskList> TaskLists => Set<TaskList>();
    public DbSet<TodoTask> Tasks => Set<TodoTask>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>()
        .HasMany(b => b.TaskLists)
        .WithOne(l => l.Board)
        .HasForeignKey(f => f.BoardId);

        modelBuilder.Entity<TaskList>()
        .HasMany(t => t.Tasks)
        .WithOne(t => t.TaskList)
        .HasForeignKey(t => t.TaskListId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TodoTask>()
        .Property(t => t.Title)
        .IsRequired();

    }

}