using Domain.DTOs;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface ITodoService
{
    Task<Todo> CreateAsync(TodoCreationDto creationDto);
    Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto parametersDto);
    Task UpdateAsync(TodoUpdateDto dto);
    Task<Todo> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}