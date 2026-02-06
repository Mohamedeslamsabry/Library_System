using Domain_Layer.Models.Book_Models;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Shared;
using Domain_Layer.Models.Users_Models;

namespace Domain_Layer.Models.Borrow_Models
{
    public class Borrow : BaseEntity
    {
        #region Properties
        public DateTime DateBorrow { get; set; }  
        public DateTime DueDate { get; set; }
        public int Amount { get; set; }
        #endregion

        #region RelationShps

        #region Tiranry

        #region Employee (R01)
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;
        #endregion

        #region User (R01)
        public int UserId { get; set; }
        public virtual Users User { get; set; } = null!;
        #endregion

        #region Book (R03)
        public int BookId { get; set; }
        public virtual Book Book { get; set; } = null!;
        #endregion

        #endregion

        #endregion
    }

}
