namespace Domain_Layer.Exceptions
{
    public sealed class FloorNotFoundException(int id) : NotFoundExceptions($"Floor With Number :{id} Is Not Found")
    {

    }
}
