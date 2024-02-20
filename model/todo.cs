using Microsoft.EntityFrameworkCore;

public class TodoContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    public string DbPath { get; private set; }

    public TodoContext()
    {
        DbPath = "./db/todo.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class TodoItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }
}

public class CreateTodoItem
{
    public string Name { get; set; }
    public bool IsComplete { get; set; }
}

