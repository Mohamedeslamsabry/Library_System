using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Shared;

namespace Domain_Layer.Models.Users_Models
{
    public class Users : BaseEntity
    {
        #region Properties
        public string User_Name { get; set; } = null!;
        public string User_Email { get; set; } = null!;
        public string User_Phone { get; set; } = null!;
        public Gender Gender { get; set; }

        #endregion

        #region RelationShips

        #region Employee(R01)
        public virtual Employee Employee { get; set; } = null!;
        public int? EmployeeId { get; set; } //Fk 
        #endregion

        #endregion
    }
}
