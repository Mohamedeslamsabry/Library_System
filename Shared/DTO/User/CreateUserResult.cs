namespace Shared.DTO.User
{
    public class CreateUserResult
    {
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }   // "DuplicatePhone", "DuplicateEmail", "EmployeeNotFound", "SaveFailed", "UnexpectedError", "UniqueConstraintViolation"
        public string? ErrorField { get; set; }  // "User_Phone" | "User_Email" | "Employee    public string? ErrorField { get; set; }  // "User_Phone" | "User_Email" | "EmployeeId"
        public string? Message { get; set; }
    }
}
