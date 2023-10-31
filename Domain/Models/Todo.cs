using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Todo
{
    public int Id { get; set; }
    public int OwnerId { get; private set; }
    public string Title { get; private set; }
    public bool IsCompleted { get; set; }


    public Todo(int ownerId, string title)
    {
        OwnerId = ownerId;
        Title = title;
    }
    
    private Todo(){}
}