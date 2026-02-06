namespace Shared.DTO.Employee
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        public AddressDTO? Address { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public int Salary { get; set; }
        public int? Bouns { get; set; }

        public string Gender { get; set; } = null!;

        public int UsersCount { get; set; }

        public int? FloorsNumberWork { get; set; }

        public int? FloorMangeNumber { get; set; }

        public int? SupervisorId { get; set; }  
        public string? SupervisorFullName { get; set; }

        public int SubordinatesCount { get; set; }

    }
}
