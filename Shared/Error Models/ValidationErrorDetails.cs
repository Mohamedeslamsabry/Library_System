namespace Shared.Error_Models
{
    public class ValidationErrorDetails
    {
        public string Field { get; set; } = null!;
        public IEnumerable<string> Errors { get; set; } = []; // Ex => Range ,, "Hambozo" , ......
    }
}
