namespace Domain_Layer.Exceptions
{ 
    public sealed class EmployeeNotFoundException(int id) : NotFoundExceptions($"Employee With Id :{id} Is Not Found")
    {

    }
}
