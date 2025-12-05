using Domain_Layer.Models.Employee_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class EmployeeSpecifcation : BaseSpecification<Employee>
    {
        public EmployeeSpecifcation(EmployeeQueryParamter employeeQuery) :
            base (P => string.IsNullOrEmpty(employeeQuery.search) || P.FirstName.ToLower().Contains(employeeQuery.search.ToLower()))
            

        {
           

            switch (employeeQuery.sort)
            {
                case EmployeeSortingSpecifications.NameAsc:
                    SetOrdery(P => P.FirstName);
                    break;
                case EmployeeSortingSpecifications.NameDesc:
                    SetOrderyDesc(P => P.FirstName);
                    break;
                case EmployeeSortingSpecifications.SalaryAsc:
                    SetOrdery(P => P.Salary);
                    break;
                case EmployeeSortingSpecifications.SalaryDesc:
                    SetOrderyDesc(P => P.Salary);
                    break;
                default:
                    break;
            }

            ApplyPagention(employeeQuery.pageSize, employeeQuery.PageIndex);
        }

       
    }
}
