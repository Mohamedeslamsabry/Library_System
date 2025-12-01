using Domain_Layer.Employee_Models;
using Domain_Layer.Shared;

namespace Domain_Layer.Users_Models
{
    public class Users : BaseEntity
    {
        #region Properties
        public string User_Name { get; set; } = null!;
        public string User_Email { get; set; } = null!;
        public string User_Phone { get; set; } = null!;
        #endregion

        #region RelationShips

        #region Employee(R01)
        public Employee Employee { get; set; } = null!;
        public int EmployeeId { get; set; } //Fk 
        #endregion

        #endregion
    }
}
