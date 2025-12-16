using Shared;
using Shared.DTO.Employee;
using Shared.Error;

namespace Service_Abstraction.Interfaces
{
    public interface IEmployeeService
    {
        #region GetAllAsync
        Task<PaginatedResult<EmployeeDTO>> GetAllAsync(EmployeeQueryParamter employeeQuery);
        #endregion

        #region GetByIdAsync
        Task<EmployeeDTO?> GetByIdAsync(int id);
        #endregion

        #region CreateAsync
        Task<CreateEmployeeResult> CreateAsync(CreateOrUpdateEmployeeDTO createEmployee);
        #endregion

        #region UpdateAsync
        Task<UpdateEmployeeResult> UpdateAsync(int id, CreateOrUpdateEmployeeDTO updateEmployee);
        #endregion

        #region DeleteAsync
        Task<Result<int>> DeleteAsync(int id); 
        #endregion
    }
}
