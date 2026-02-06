using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Employee
{
    public class AddressDTO
    {
        [Required(ErrorMessage = "City  Is Requierd")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must Be Bettwen 2 and 30 character")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contaian Only Letters And Space")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Country  Is Requierd")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Country Must Be Bettwen 2 and 30 character")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Country Can Contaian Only Letters And Space")]
        public string Country { get; set; } = null!;
        public int? BuildingNumber { get; set; }
        public string? Street { get; set; }

        [Required(ErrorMessage = "Area  Is Requierd")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Area Must Be Bettwen 2 and 30 character")]
        public string Area { get; set; } = null!;
    }
}
