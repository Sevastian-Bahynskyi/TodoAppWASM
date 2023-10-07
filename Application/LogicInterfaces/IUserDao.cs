using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface IUserDao
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User> CreateAsync(User user);
    Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters);
    Task<User> GetByIdAsync(int id);
}