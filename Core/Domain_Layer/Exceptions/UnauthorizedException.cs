namespace Domain_Layer.Exceptions
{
    public class UnauthorizedException(string Message = "Invalid Email Or Passowrd") : Exception(Message)
    {
    }
}
