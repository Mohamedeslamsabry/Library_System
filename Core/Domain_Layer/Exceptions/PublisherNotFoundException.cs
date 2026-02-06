namespace Domain_Layer.Exceptions
{
    public sealed class PublisherNotFoundException(int id) : NotFoundExceptions($"Publisher With Id :{id} Is Not Found")
    {

    }
}
