using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class UserHttpClient: IUserService
{
    private readonly HttpClient client;

    public UserHttpClient(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<User> CreateAsync(UserCreationDto creationDto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/users", creationDto);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        User user = JsonSerializer.Deserialize<User>(result,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;

        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync(SearchUserParametersDto parametersDto)
    {
        string uri = "/users";
        if (!string.IsNullOrEmpty(parametersDto.UsernameContains))
            uri += $"?username={parametersDto.UsernameContains}";
        HttpResponseMessage response = await client.GetAsync(uri);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        IEnumerable<User> users = JsonSerializer.Deserialize<IEnumerable<User>>(result,
            new JsonSerializerOptions()
            {

                PropertyNameCaseInsensitive = true
            })!;
        return users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"/users/{id}");
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        User user = JsonSerializer.Deserialize<User>(result,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;

        return user;
    }
}