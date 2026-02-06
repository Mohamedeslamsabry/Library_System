namespace Domain_Layer.Exceptions
{
    public class BadRequestException(List<string> Errors) : Exception("Validtion Field")
    {
        public List<string> Errors { get; } = Errors;
    }
}
