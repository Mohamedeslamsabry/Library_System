using System.Net;

namespace Shared.Error_Models
{
    public class ValidationErrorToReturn
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
        public string ErrorMessage { get; set; } = "Validtion Failed";
        public IEnumerable<ValidationErrorDetails> validtionErrors { get; set; } = [];
    }
}
