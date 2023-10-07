using Domain.DTOs;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface IUserService
{
    Task<User> CreateAsync(UserCreationDto creationDto);
    Task<IEnumerable<User>> GetAllAsync(SearchUserParametersDto parametersDto);
}