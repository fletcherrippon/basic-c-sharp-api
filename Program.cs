using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

await using var todosDB = new TodoContext();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basic API in C#", Description = "Basics of ASP.NET", Version = "v1" });
   });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
   });

app.MapGet("/", () => "Hello World!!!");
app.MapGet("/json", () => new { Name = "JSON", Description = "This is an example JSON object response" });
// Todos
app.MapGet("/todos", () =>
{
    var todos =
        from todo in todosDB.TodoItems
        select new { todo.Id, todo.Name, todo.IsComplete };

    return Results.Ok(todos);
});

app.MapGet("/todos/{id}", ([FromRoute] int id) =>
{
    var todo = todosDB.TodoItems.Find(id);
    if (todo == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(todo);
});

app.MapPost("/todos", ([FromBody] CreateTodoItem todoData) =>
{
    var newTodo = new TodoItem
    {
        Name = todoData.Name,
        IsComplete = todoData.IsComplete
    };

    todosDB.TodoItems.Add(newTodo);
    todosDB.SaveChanges();

    return Results.Created($"/todos/{newTodo.Id}", newTodo);
});

app.MapPut("/todos/{id}/toggle-complete", ([FromRoute] int id) =>
{
    var todo = todosDB.TodoItems.Find(id);
    if (todo == null)
    {
        return Results.NotFound();
    }

    todo.IsComplete = !todo.IsComplete;

    todosDB.SaveChanges();

    return Results.NoContent();
});

app.Run();

