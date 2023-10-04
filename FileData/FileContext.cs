using System.Text.Json;
using Shared;

namespace FileData;

public class FileContext
{
    private const string FILE_PATH = "data.json";
    private DataContainer? dataContainer;

    public ICollection<Todo> Todos
    {
        get
        {
            LoadData();
            return dataContainer!.Todos;
        }
    }


    public ICollection<User> Users
    {
        get
        {
            LoadData();
            return dataContainer!.Users;
        }
    }


    private void LoadData()
    {
        if (dataContainer != null) return;

        if (!File.Exists(FILE_PATH))
        {
            dataContainer = new()   
            {
                Todos = new List<Todo>(),
                Users = new List<User>()
            };
            return;
        }

        string content = File.ReadAllText(FILE_PATH);
        dataContainer = JsonSerializer.Deserialize<DataContainer>(content);
    }

    public void SaveChanges()
    {
        dataContainer.Todos = dataContainer.Todos.OrderBy(t => t.Id).ToList();
        dataContainer.Users = dataContainer.Users.OrderBy(u => u.Id).ToList();
        string jsonObject = JsonSerializer.Serialize(dataContainer, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        File.WriteAllText(FILE_PATH, jsonObject);
        dataContainer = null;
    }
}