using Microsoft.EntityFrameworkCore;

namespace BasicAPI.Todo
{
    public class TodoContext : DbContext
    {
        public DbSet<TodoModel> TodoItems { get; set; }

        public string DbPath { get; private set; } = "./db/todo.db";

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder options) { }
    }

    public class TodoModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
    }

}