using Domain_Layer.Book_Models;
using Domain_Layer.Employee_Models;
using Domain_Layer.Shared;
using Domain_Layer.Users_Models;

namespace Domain_Layer.Borrow_Models
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
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        #endregion

        #region User (R01)
        public int UserId { get; set; }
        public Users User { get; set; } = null!;
        #endregion

        #region Book (R03)
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        #endregion

        #endregion

        #endregion

    }

}
