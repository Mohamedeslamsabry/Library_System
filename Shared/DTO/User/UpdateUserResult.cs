namespace Shared.DTO.User
{
    public class UpdateUserResult
    {
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }   // "NotFound", "DuplicatePhone", "DuplicateEmail", "EmployeeNotFound", "SaveFailed", "UniqueConstraintViolation", "UnexpectedError"
        public string? ErrorField { get; set; }  // "User_Phone" | "User_Email" | "EmployeeId"
        public string? Message { get; set; }
    }

}
