namespace Shared.DTO.Employee
{

    public class UpdateEmployeeResult
    {
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }   // e.g. "NotFound", "DuplicatePhone", "DuplicateEmail", "SupervisorNotFound", "FloorNotFound", "SelfSupervisor", "ConcurrencyConflict"
        public string? ErrorField { get; set; }  // e.g. "PhoneNumber", "Email", "SupervisorId", "floorsNumberWork"
        public string? Message { get; set; }
    }
}