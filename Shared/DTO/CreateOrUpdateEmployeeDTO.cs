using System.ComponentModel.DataAnnotations;

namespace Shared.DTO
{
    public class CreateOrUpdateEmployeeDTO
    {
        #region Name
        [Required(ErrorMessage = "First Name Is Requierd")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "FirstName Must Be Bettwen 2 and 50 character")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "FirstName Can Contaian Only Letters And Space")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name Is Requierd")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "LastName Must Be Bettwen 2 and 50 character")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "LastName Can Contaian Only Letters And Space")]
        public string LastName { get; set; } = null!;

        #endregion

        #region Email
        [Required(ErrorMessage = "Email Is Requierd")]
        [EmailAddress(ErrorMessage = "Email Is Not vaild Format")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Email Must Be Bettwen 5 and 100 character")]
        public string Email { get; set; } = null!;
        #endregion

        #region PhoneNumber
        [Required(ErrorMessage = "Phone Is Requierd")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invaild Phone Format")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Egyption Phone Only")]
        public string PhoneNumber { get; set; } = null!;
        #endregion

        #region DateOfBirth
        [Required(ErrorMessage = "DateOfBirth Is Requierd")]
        [DataType(DataType.Date)]

        public DateOnly DateOfBirth { get; set; }
        #endregion

        #region Gender
        [Required(ErrorMessage = "Gender Is Requierd")]
        public GenderDto Gender { get; set; }
        #endregion

        #region Address

        #region BuildingNo
        [Required(ErrorMessage = "Address  Is Requierd")]
        public AddressDTO Address { get; set; } = null!;
        #endregion

        //#region Street
        //[Required(ErrorMessage = "Country  Is Requierd")]
        //[StringLength(30, MinimumLength = 2, ErrorMessage = "Country Must Be Bettwen 2 and 30 character")]
        //[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Country Can Contaian Only Letters And Space")]

        //public string Country { get; set; } = null!;
        //#endregion

        //#region City
        //[Required(ErrorMessage = "City  Is Requierd")]
        //[StringLength(30, MinimumLength = 2, ErrorMessage = "City Must Be Bettwen 2 and 30 character")]
        //[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contaian Only Letters And Space")]

        //public string City { get; set; } = null!;
        //#endregion

        #endregion

        [Range(4000,int.MaxValue )]
        public int Salary { get; set; }
        public int? Bouns { get; set; }

      

        // Relationships (اختياري)
        // R02: الموظف يعمل في دور (Floor Work)
        public int? floorsNumberWork { get; set; }

        //// رقم الدور للعرض فقط (لو بتسجّله مع الإنشاء)
        //public int? FloorsNumber { get; set; }

        // R03: هل الموظف يدير دور بعينه؟
        public int? floorMangeNumber { get; set; }

        // R04: المشرف (Self-Reference)
        public int? SupervisorId { get; set; }
    }
}
