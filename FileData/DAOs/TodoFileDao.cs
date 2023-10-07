using Application.LogicInterfaces;
using Domain.CustomExceptions;
using Domain.DTOs;
using Domain.Models;

namespace FileData;

public class TodoFileDao: ITodoDao
{
    private readonly FileContext context;

    
    public TodoFileDao(FileContext context)
    {
        this.context = context;
    }
    
    
    public Task<Todo> CreateAsync(Todo todo)
    {
        int todoId = 1;

        if (context.Todos.Any())
        {
            todoId = context.Todos.Max(t => t.Id) + 1;
        }

        todo.Id = todoId;
        context.Todos.Add(todo);
        context.SaveChanges();
        return Task.FromResult(todo);
    }

    public Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto parametersDto)
    {
        IEnumerable<Todo> todos = context.Todos
            .Where(t => string.IsNullOrEmpty(parametersDto.Username) || t.Owner.Username.Equals(parametersDto.Username, StringComparison.OrdinalIgnoreCase))
            .Where(t => string.IsNullOrEmpty(parametersDto.TitleContains) || t.Title.Contains(parametersDto.TitleContains, StringComparison.OrdinalIgnoreCase))
            .Where(t => parametersDto.UserId == null || t.Owner.Id == parametersDto.UserId)
            .Where(t => parametersDto.CompletedStatus == null || t.IsCompleted == parametersDto.CompletedStatus);

        
        return Task.FromResult(todos); // why do we use this weird method and not using async keyword instead
    }

    public Task<Todo?> GetByTitleAndUserIdAsync(string title, int userId)
    {
        return Task.FromResult(context.Todos.FirstOrDefault(t => t.Title.Equals(title) && t.Owner.Id == userId));
    }


    public Task UpdateAsync(Todo todo)
    {
        Todo? existing = context.Todos.FirstOrDefault(t => t.Id == todo.Id);
        if (existing is null)
            throw new TodoNotFoundException(todo.Id);

        context.Todos.Remove(existing);
        context.Todos.Add(todo);
        context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Todo todo)
    {
        context.Todos.Remove(todo);
        context.SaveChanges();

        return Task.CompletedTask;
    }

    public Task<Todo?> GetByIdAsync(int id)
    {
        return Task.FromResult(context.Todos.FirstOrDefault(t => t.Id == id));
    }
}