namespace Domain_Layer.Exceptions
{
    public sealed class UserNotFoundException(int id) : NotFoundExceptions($"User With Id :{id} Is Not Found")
    {

    }

}
