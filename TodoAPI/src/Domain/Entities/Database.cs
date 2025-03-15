using Microsoft.EntityFrameworkCore;
using TodoAPI.src.Domain.Entities;

public class TodoTaskDb : DbContext
{
    public TodoTaskDb(DbContextOptions<TodoTaskDb> options)
        : base(options) { }

    public DbSet<TodoTask> tasks => Set<TodoTask>();
}