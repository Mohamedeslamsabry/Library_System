namespace Domain_Layer.Exceptions
{
    public sealed class BookNotFoundException(int id) : NotFoundExceptions($"Book With Id :{id} Is Not Found")
    {

    }
}
