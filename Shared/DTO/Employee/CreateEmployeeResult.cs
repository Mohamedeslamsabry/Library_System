namespace Shared.DTO.Employee
{

    public class CreateEmployeeResult
    {
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }       // e.g. "DuplicatePhone", "DuplicateEmail"
        public string? ErrorField { get; set; }      // "PhoneNumber" | "Email" | null
        public string? Message { get; set; }
    }
}
