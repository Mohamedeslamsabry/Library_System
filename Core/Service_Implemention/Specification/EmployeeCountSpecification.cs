using Domain_Layer.Models.Employee_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class EmployeeCountSpecification : BaseSpecification<Employee>
    {
        public EmployeeCountSpecification(EmployeeQueryParamter employeeQuery) :
            base(P => (string.IsNullOrEmpty(employeeQuery.search) || P.FirstName.ToLower().Contains(employeeQuery.search.ToLower()))
            )

        {



        }
    }
}
