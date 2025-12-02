using Domain_Layer.Floors_Models;
using Domain_Layer.Shared;
using Domain_Layer.Users_Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain_Layer.Employee_Models
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
        public int Bouns { get; set; }
        public Gender Gender { get; set; }  
        #endregion

        #region RelationShips

        #region Users(R01) (Record)
        public ICollection<Users> Users { get; set; } = new HashSet<Users>();
        #endregion

        #region Floors(R02)(Work)

        [InverseProperty(nameof(Floors.employeesWork))]
        public Floors Floors { get; set; } = null!;
        public int? FloorsNumber { get; set; }
        #endregion

        #region Floors (R03) (Mange)
        [InverseProperty(nameof(Floors.EmployeeMange))]
        public Floors FloorsMange { get; set; } = null!;

        #endregion

        #region SuperVisor (R04)

        // العلاقة مع المشرف
        public Employee Supervisor { get; set; } = null!;
        public int? SupervisorId { get; set; } //Fk

        // العلاقة مع الموظفين اللي تحت المشرف ده
        public ICollection<Employee> Subordinates { get; set; } = new HashSet<Employee>();
        #endregion

        #endregion
    }
}
