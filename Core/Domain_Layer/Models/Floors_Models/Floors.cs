using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Shared;
using Domain_Layer.Models.Shelf_Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain_Layer.Models.Floors_Models
{
    public class Floors : BaseEntity
    {
        #region Properties
        //public int FloorNumber { get; set; } // Id    
        public int Number_of_Blocks { get; set; }
        //public DateTime HiringDate { get; set; } // createdAt
        #endregion

        #region RelationShips

        #region Employee (R01) Work

        [InverseProperty(nameof(Employee.Floors))]
        public virtual ICollection<Employee> employeesWork { get; set; } = new HashSet<Employee>();

        #endregion

        #region Employee (R02) Mange
        [InverseProperty(nameof(Employee.FloorsMange))]
        public virtual Employee EmployeeMange { get; set; } = null!;
        public int? EmployeeMangeId { get; set; }
        #endregion

        #region Shelf(R03) (Located)
        public virtual ICollection<Shelf> Shelfs { get; set; } = new HashSet<Shelf>();
        #endregion

        #endregion
    }
}
