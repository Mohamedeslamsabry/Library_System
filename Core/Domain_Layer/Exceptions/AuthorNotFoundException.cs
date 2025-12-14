namespace Domain_Layer.Exceptions
{
    public sealed class AuthorNotFoundException(int id) : NotFoundExceptions($"Author With Id :{id} Is Not Found")
    {

    }
}
