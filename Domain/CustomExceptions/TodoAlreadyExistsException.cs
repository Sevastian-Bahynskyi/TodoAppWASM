namespace Domain.DTOs;

public class TodoAlreadyExistsException: Exception
{
    public override string Message => "Todo is already assigned to the user.";
}