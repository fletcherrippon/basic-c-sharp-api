using BasicAPI.Todo;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TodoContext")));

builder.Services.AddScoped<TodoService>();

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

// Define the routes for the API
TodoController.DefineRoutes(app);

app.Run();

