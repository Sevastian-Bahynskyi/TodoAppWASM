using Application.LogicInterfaces;
using Domain.CustomExceptions;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class TodoLogic: ITodoLogic
{
    private readonly ITodoDao todoDao;
    private readonly IUserDao userDao;


    public TodoLogic(ITodoDao todoDao, IUserDao userDao)
    {
        this.todoDao = todoDao;
        this.userDao = userDao;
    }

    
    public async Task<Todo> CreateAsync(TodoCreationDto dto)
    {
        User? user = await userDao.GetByIdAsync(dto.OwnerId);
        if (user is null)
            throw new UserNotFoundException();
        Todo? existingTodo = await todoDao.GetByTitleAndUserIdAsync(dto.Title, user.Id);

        if (existingTodo is not null)
            throw new TodoAlreadyExistsException();
        
        ValidateTodo(dto);
        Todo toCreate = new(user, dto.Title);
        Todo todo = await todoDao.CreateAsync(toCreate);
        
        return todo;
    }

    public async Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto parametersDto)
    {
        return await todoDao.GetAsync(parametersDto);
    }

    public async Task<Todo> GetByIdAsync(int id)
    {
        var todo = await todoDao.GetByIdAsync(id);
        if (todo == null)
            throw new TodoNotFoundException(id);

        return todo;
    }

    public async Task UpdateAsync(TodoUpdateDto updateDto)
    {
        Todo? existing = await todoDao.GetByIdAsync(updateDto.Id);

        if (existing is null)
            throw new TodoNotFoundException();

        if (updateDto.IsCompleted != null && existing.IsCompleted && !(bool)updateDto.IsCompleted)
        {
            throw new Exception("Cannot un-complete a completed Todo");
        }
        
        existing.Owner = (await userDao.GetByIdAsync(updateDto.OwnerId ?? existing.Owner.Id))!;
        existing.Title = updateDto.Title ?? existing.Title;
        existing.IsCompleted = updateDto.IsCompleted ?? existing.IsCompleted;

        await todoDao.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        Todo? existing = await todoDao.GetByIdAsync(id);

        if (existing is null)
            throw new TodoNotFoundException(id);

        if (!existing.IsCompleted)
            throw new TodoNotCompletedException(id);
        
        await todoDao.DeleteAsync(existing);
    }

    private static void ValidateTodo(TodoCreationDto dto)
    {
        string todoTitle = dto.Title;
        if (string.IsNullOrEmpty(todoTitle) && todoTitle.Length is < 3 or > 20)
            throw new InvalidTodoTitleLengthException();
    }
}