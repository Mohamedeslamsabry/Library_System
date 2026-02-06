namespace Domain_Layer.Exceptions
{
    public sealed class ShelfNotFoundException(int id) : NotFoundExceptions($"Shelf With Code :{id} Is Not Found")
    {

    }
}
