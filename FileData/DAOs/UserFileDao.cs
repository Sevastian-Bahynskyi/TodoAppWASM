using Application.LogicInterfaces;
using Domain.CustomExceptions;
using Domain.DTOs;
using Shared;

namespace FileData;

public class UserFileDao: IUserDao
{
    private readonly FileContext context;


    public UserFileDao(FileContext context)
    {
        this.context = context;
    }
    public Task<User?> GetByUsernameAsync(string username)
    {
        User? existing = context.Users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
        );
        return Task.FromResult(existing);
    }

    public Task<User> CreateAsync(User user)
    {
        int userId = 1;
        if (context.Users.Any())
        {
            userId = context.Users.Max(u => u.Id);
            userId++;
        }

        user.Id = userId;
        context.Users.Add(user);
        context.SaveChanges();

        return Task.FromResult(user);
    }

    public Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
    {
        IEnumerable<User> users = context.Users.AsEnumerable();
        if (searchParameters.UsernameContains is not null)
            users = context.Users.Where(u =>
                u.Username.Contains(searchParameters.UsernameContains, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(users);
    }

    public Task<User> GetByIdAsync(int id)
    {
        User? existing = context.Users.FirstOrDefault(u => u.Id == id);
        if (existing == null)
            throw new UserNotFoundException();
        
        return Task.FromResult(existing);
    }
}