using Domain.DTOs;
using Shared;

namespace Application.LogicInterfaces;

public interface ITodoLogic
{
    Task<Todo> CreateAsync(TodoCreationDto createDto);
    Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto parametersDto);
    Task<Todo> GetByIdAsync(int id);
    Task UpdateAsync(TodoUpdateDto updateDto);
    Task DeleteAsync(int id);
}