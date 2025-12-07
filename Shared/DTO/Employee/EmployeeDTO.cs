namespace Shared.DTO.Employee
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        // Basic info
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        // Address
        public AddressDTO? Address { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public int Salary { get; set; }
        public int? Bouns { get; set; }

        public string Gender { get; set; } = null!;

        // Users (R01)
        public int UsersCount { get; set; }

        // Floors (R02) Work
        public int? FloorsNumberWork { get; set; }

        // Floors (R03) Manage
        public int? FloorMangeNumber { get; set; }

        // Supervisor (R04)
        public int? SupervisorId { get; set; }  
        public string? SupervisorFullName { get; set; }

        // Subordinates
        public int SubordinatesCount { get; set; }

    }
}
