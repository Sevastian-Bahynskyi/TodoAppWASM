namespace Domain.CustomExceptions;

public class UnavailableUsernameException: Exception
{
    public override string Message => "Username already taken!";
}