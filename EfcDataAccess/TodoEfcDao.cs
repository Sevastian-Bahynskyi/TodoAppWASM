using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess;

public class TodoEfcDao : ITodoDao
{
    private readonly TodoContext context;

    public TodoEfcDao(TodoContext context)
    {
        this.context = context;
    }
    public async Task<Todo> CreateAsync(Todo todo)
    {
        EntityEntry<Todo> toCreate = await context.Todos.AddAsync(todo);
        await context.SaveChangesAsync();
        return toCreate.Entity;
    }

    public async Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto parametersDto)
    {
        IQueryable<Todo> todosQueryable = context.Todos.AsQueryable();
        foreach (var field in parametersDto.GetType().GetFields())
        {
            if (field.GetValue(parametersDto) != null)
                todosQueryable = context.Todos
                    .Where(t => t
                        .GetType().GetFields()
                        .Where(f => f.GetValue(f) != null)
                        .Where(f => f.GetValue(f) is string && !string.IsNullOrEmpty((string)f.GetValue(f)!))
                        .Where(f => f.GetType() == field.GetType())
                        .Any(f => f.GetValue(f)!.Equals(field.GetValue(parametersDto))));
        }

        return await todosQueryable.ToListAsync();
    }

    public async Task<Todo?> GetByTitleAndUserIdAsync(string title, int userId)
    {
        return await context.Todos
            .Where(t => t.Title.Equals(title))
            .Where(t => t.Owner.Id == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<Todo?> GetByIdAsync(int id)
    {
        return await context.Todos
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Todo todo)
    {
        context.Todos.Update(todo);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Todo todo)
    {
        context.Todos.Remove(todo);
        await context.SaveChangesAsync();
    }
}