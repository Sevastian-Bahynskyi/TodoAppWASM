namespace Domain.CustomExceptions;

public class InvalidUsernameLengthException: Exception
{
    public override string Message => "Username must be greater than 3 and less than 16 characters";
}