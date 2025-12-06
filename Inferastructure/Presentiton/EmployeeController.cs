using Microsoft.AspNetCore.Mvc;
using Service_Abstraction.Interfaces;
using Shared;
using Shared.DTO;

namespace Presentiton
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController(IEmployeeService _employeeService) : ControllerBase
    {
        #region Get All Employee

        [HttpGet]
        //https://localhost:7063/api/Employee
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees([FromQuery] EmployeeQueryParamter employeeQuery)
        {
            var Employees = await _employeeService.GetAllAsync(employeeQuery);
            return Ok(Employees);
        }
        #endregion  

        #region Get Employee By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return Ok(employee);
        }
        #endregion

        #region Create Employee
        [HttpPost("Create")]
        public async Task<ActionResult<bool>> CreateEmployee([FromBody] CreateOrUpdateEmployeeDTO createEmployeeDTO)
        {
            var employee = await _employeeService.CreateAsync(createEmployeeDTO);
            return Ok(employee);
        }
        #endregion

        #region Update Employee
        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<bool>> UpdateEmployee([FromRoute] int id, CreateOrUpdateEmployeeDTO updateEmployeeDTO)
        {
            var employee = await _employeeService.UpdateAsync(id, updateEmployeeDTO);
            return Ok(employee);
        }
        #endregion

        #region DeleteEmployee

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> DeleteEmployee([FromRoute] int id)
        {
            var employee = await _employeeService.DeleteAsync(id);
            return Ok(employee);
        }
        #endregion
    }
}


