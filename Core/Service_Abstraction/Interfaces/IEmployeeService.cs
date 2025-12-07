using Shared;
using Shared.DTO.Employee;

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
        Task<bool> CreateAsync(CreateOrUpdateEmployeeDTO createEmployee);
        #endregion

        #region UpdateAsync
        Task<bool> UpdateAsync(int id, CreateOrUpdateEmployeeDTO updateEmployee);
        #endregion

        #region DeleteAsync
        Task<bool> DeleteAsync(int id); 
        #endregion

    }
}
