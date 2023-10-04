namespace Domain.CustomExceptions;

public class InvalidTodoTitleLengthException: Exception
{
    public override string Message => "Todo's title length must be between 3 and 16";
}