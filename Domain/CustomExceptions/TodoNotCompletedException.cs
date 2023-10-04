namespace Domain.CustomExceptions;

public class TodoNotCompletedException: Exception
{
    public override string Message => message;
    private string message = "Todo is not completed exception.";

    public TodoNotCompletedException(int id)
    {
        message = $"Todo with id {id} is not completed.";
    }
}