namespace Domain.CustomExceptions;

public class UserNotFoundException: Exception
{
    public override string Message => "User with such id doesn't exist";
}