namespace BasicAPI.Todo
{
    public class TodoService
    {
        private readonly TodoContext _context;

        public TodoService(TodoContext context)
        {
            _context = context;
        }

        public List<TodoModel> GetAll()
        {
            var todos = _context.TodoItems;

            return todos.ToList();
        }

        public TodoModel? Get(int id)
        {
            var todo = _context.TodoItems.Find(id);

            if (todo == null)
            {
                return null;
            }

            return todo;
        }

        public TodoModel Create(CreateTodoInput todoData)
        {
            var newTodo = new TodoModel
            {
                Name = todoData.Name,
                IsComplete = todoData.IsComplete
            };

            _context.TodoItems.Add(newTodo);
            _context.SaveChanges();

            return newTodo;
        }

        public TodoModel? ToggleComplete(int id)
        {
            var todo = Get(id);

            if (todo == null)
            {
                return null;
            }

            todo.IsComplete = !todo.IsComplete;

            _context.SaveChangesAsync();

            return todo;
        }
    }
}