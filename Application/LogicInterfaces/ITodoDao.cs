using System.Collections;
using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface ITodoDao
{
    Task<Todo> CreateAsync(Todo todo);
    Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto parametersDto);
    Task<Todo?> GetByTitleAndUserIdAsync(string title, int userId);
    Task<Todo?> GetByIdAsync(int id);
    Task UpdateAsync(Todo todo);
    Task DeleteAsync(Todo todo);
}