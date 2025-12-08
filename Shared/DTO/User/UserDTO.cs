namespace Shared.DTO.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string User_Name { get; set; } = null!;
        public string User_Email { get; set; } = null!;
        public string User_Phone { get; set; } = null!;
        public string Gender { get; set; } = null!; 

        public int? EmployeeId { get; set; } 
        public EmployeeShortDto? Employee { get; set; }
    }

    public class EmployeeShortDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
    }
