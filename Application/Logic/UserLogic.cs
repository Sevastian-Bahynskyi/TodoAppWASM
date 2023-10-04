using Application.LogicInterfaces;
using Domain.CustomExceptions;
using Domain.DTOs;
using Shared;

namespace Application.Logic;

public class UserLogic: IUserLogic
{
    private readonly IUserDao userDao;

    public UserLogic(IUserDao userDao)
    {
        this.userDao = userDao;
    }
    
    public async Task<User> CreateAsync(UserCreationDto userToCreate)
    {
        User? existing = await userDao.GetByUsernameAsync(userToCreate.Username);
        if (existing != null)
            throw new UnavailableUsernameException();

        ValidateData(userToCreate);

        User toCreate = new User
        {
            Username = userToCreate.Username
        };

        User created = await userDao.CreateAsync(toCreate);
        return created;
    }

    public Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
    {
        return userDao.GetAsync(searchParameters);
    }

    private void ValidateData(UserCreationDto userToCreate)
    {
        string userName = userToCreate.Username;

        if (userName.Length is < 3 or > 16)
            throw new InvalidUsernameLengthException();
    }
}