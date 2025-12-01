using Domain_Layer.Employee_Models;
using Domain_Layer.Shared;
using Domain_Layer.Shelf_Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain_Layer.Floors_Models
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

        [InverseProperty(nameof(employeesWork))]
        public ICollection<Employee> employeesWork { get; set; } = new HashSet<Employee>();

        #endregion

        #region Employee (R02) Mange
        [InverseProperty(nameof(EmployeeMange))]
        public Employee EmployeeMange { get; set; } = null!;
        public int EmployeeMangeId { get; set; }
        #endregion

        #region Shelf(R03) (Located)
        public ICollection<Shelf> Shelfs { get; set; } = new HashSet<Shelf>();
        #endregion

        #endregion
    }
}
