using Microsoft.AspNetCore.Mvc;

namespace BasicAPI.Todo
{
    public static class TodoController
    {
        public static void DefineRoutes(WebApplication app)
        {

            app.MapGet("/todos", (TodoService todoService) =>
            {
                var res = todoService.GetAll();
                return Results.Ok(res);
            });

            app.MapGet("/todos/{id}", (TodoService todoService, [FromRoute] int id) =>
            {
                var todo = todoService.Get(id);
                return Results.Ok(todo);
            });

            app.MapPost("/todos", (TodoService todoService, [FromBody] CreateTodoInput todoData) =>
            {
                var newTodo = todoService.Create(todoData);

                return Results.Created($"/todos/{newTodo.Id}", newTodo);
            });

            app.MapPut("/todos/{id}/toggle-complete", (TodoService todoService, [FromRoute] int id) =>
            {
                todoService.ToggleComplete(id);

                return Results.NoContent();
            });
        }

    }
}