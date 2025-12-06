using Domain_Layer.Models.Floors_Models;
using Domain_Layer.Models.Shared;
using Domain_Layer.Models.Users_Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain_Layer.Models.Employee_Models
{
    public class Employee : BaseEntity
    {
        #region Properties
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Address Address { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public int Salary { get; set; }
        public int? Bouns { get; set; }
        public Gender Gender { get; set; }
        #endregion

        #region RelationShips

        #region Users(R01) (Record)
        public virtual ICollection<Users> Users { get; set; } = new HashSet<Users>();
        #endregion

        #region Floors(R02)(Work)

        [InverseProperty(nameof(Floors.employeesWork))]
        public virtual Floors? Floors { get; set; }
        public int? FloorsNumber { get; set; }
        #endregion

        #region Floors (R03) (Mange)
        [InverseProperty(nameof(Floors.EmployeeMange))]
        public virtual Floors? FloorsMange { get; set; }

        #endregion

        #region SuperVisor (R04)

        // العلاقة مع المشرف
        public virtual Employee? Supervisor { get; set; }
        public int? SupervisorId { get; set; } //Fk

        // العلاقة مع الموظفين اللي تحت المشرف ده
        public virtual ICollection<Employee> Subordinates { get; set; } = new HashSet<Employee>();
        #endregion

        #endregion
    }
}
