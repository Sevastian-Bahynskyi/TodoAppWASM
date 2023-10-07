using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class TodoHttpClient : ITodoService
{
    private readonly HttpClient client;

    public TodoHttpClient(HttpClient client)
    {
        this.client = client;
    }
    
    
    public async Task<Todo> CreateAsync(TodoCreationDto creationDto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/todos", creationDto);
        string responseMessage = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(responseMessage);
            throw new Exception(responseMessage);
        }

        Todo todo = JsonSerializer.Deserialize<Todo>(responseMessage,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;
        return todo;
    }

    public async Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto parametersDto)
    {
        string uri = "/todos";
        string query = ConstructQuery(parametersDto);

        HttpResponseMessage response = await client.GetAsync(uri + query);
        string resultMessage = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(resultMessage);
        }
        
        
        return JsonSerializer.Deserialize<IEnumerable<Todo>>(resultMessage,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;
    }
    
    private static string ConstructQuery(SearchTodoParametersDto p)
    {
        string query = "";
        if (!string.IsNullOrEmpty(p.Username))
        {
            query += $"?username={p.Username}";
        }

        if (p.UserId != null)
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"userid={p.UserId}";
        }

        if (p.CompletedStatus != null)
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"completedStatus={p.CompletedStatus}";
        }

        if (!string.IsNullOrEmpty(p.TitleContains))
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"titleContains={p.TitleContains}";
        }

        return query;
    }
}