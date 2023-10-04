namespace Domain.CustomExceptions;

public class TodoNotFoundException: Exception
{
    public override string Message => message;
    private string message = "Todo with such id is not found.";

    public TodoNotFoundException(int todoId)
    {
        message = $"Todo with id {todoId} does not exist.";
    }

    public TodoNotFoundException()
    {
        
    }
}