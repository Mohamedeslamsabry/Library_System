using Shared.DTO.Employee;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.User
{
    public class CreateOrUpdateUserDTO
    {

        [Required(ErrorMessage = "Name Is Requierd")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name Must Be Bettwen 2 and 50 character")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name Can Contaian Only Letters And Space")]
        public string User_Name { get; set; } = null!;

        [Required(ErrorMessage = "Email Is Requierd")]
        [EmailAddress(ErrorMessage = "Email Is Not vaild Format")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Email Must Be Bettwen 5 and 100 character")]
        public string User_Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Is Requierd")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invaild Phone Format")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Egyption Phone Only")]
        public  string User_Phone { get; set; } = null!;

        [Required(ErrorMessage = "Gender Is Requierd")]
        public GenderDto Gender { get; set; }
        public int? EmployeeId { get; set; }

    }
}
