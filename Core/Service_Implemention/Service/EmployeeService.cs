using AutoMapper;
using AutoMapper.Execution;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Floors_Models;
using Microsoft.EntityFrameworkCore;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO;
using System.Drawing;

namespace Service_Implemention.Service
{
    public class EmployeeService(IUnitOfWork _unitOfWork, IMapper _mapper) : IEmployeeService
    {
        #region GetAllAsync
        public async Task<PaginatedResult<EmployeeDTO>> GetAllAsync(EmployeeQueryParamter employeeQuery)
        {
            var Specification = new EmployeeSpecifcation(employeeQuery);
            var Employees = await _unitOfWork.GetRepoartory<Employee>().GetAllAsync(Specification);
            var EmployeeDto = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDTO>>(Employees);

            #region Paggention
            var spec = new EmployeeCountSpecification(employeeQuery);
            var TotalCount = await _unitOfWork.GetRepoartory<Employee>().CountAsync(spec);
            #endregion

            return new PaginatedResult<EmployeeDTO>(TotalCount, Employees.Count(), employeeQuery.PageIndex, EmployeeDto);
        }
        #endregion

        #region GetByIdAsync
        public async Task<EmployeeDTO?> GetByIdAsync(int id)
        {
            var employee = await _unitOfWork.GetRepoartory<Employee>().GetByIdAsync(id);
            return employee == null ? null : _mapper.Map<EmployeeDTO>(employee);
        }
        #endregion

        #region CreateAsync
        public async Task<bool> CreateAsync(CreateOrUpdateEmployeeDTO createEmployee)
        {
            try
            {
                bool EmailIsExist = _unitOfWork.GetRepoartory<Employee>().GetAllAsync(X => X.Email == createEmployee.Email).Result.Any();
                bool PhoneIsExist = _unitOfWork.GetRepoartory<Employee>().GetAllAsync(X => X.PhoneNumber == createEmployee.PhoneNumber).Result.Any();
                if (EmailIsExist || PhoneIsExist)
                {
                    return false;
                }
                var employee = _mapper.Map<CreateOrUpdateEmployeeDTO, Employee>(createEmployee);

                // ✅ Business Rule: الاسم مطلوب
                if (string.IsNullOrWhiteSpace(createEmployee.FirstName))
                    throw new ArgumentException("Employee name is required.");


                //// ✅ Business Rule: المدير لا يمكن أن يكون نفسه
                //if (createEmployee.SupervisorId.HasValue && createEmployee.SupervisorId == employee.SupervisorId)
                //    throw new ArgumentException("Employee cannot be their own supervisor.");



                // ✅ تحقق من وجود المدير لو تم إدخاله
                if (createEmployee.SupervisorId.HasValue && !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == createEmployee.SupervisorId))
                    throw new ArgumentException("Supervisor does not exist.");

                // ✅ تحقق من وجود الدور لو تم إدخاله
                if (createEmployee.floorsNumberWork.HasValue && !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(f => f.FloorsNumber == createEmployee.floorsNumberWork))
                    throw new ArgumentException("Floor does not exist.");

                // ✅ تحقق من وجود الدور لو تم إدخاله
                if (createEmployee.floorMangeNumber.HasValue && !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(f => f.FloorsNumber == createEmployee.floorMangeNumber))
                    throw new ArgumentException("Floor does not exist.");



                await _unitOfWork.GetRepoartory<Employee>().AddAsync(employee);
                var isCreated = await _unitOfWork.SaveChangesAsync() > 0;
                if (!isCreated)
                {
                    return false;
                }
                else
                {
                    return isCreated;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        #region UpdateAsync
        public async Task<bool> UpdateAsync(int id, CreateOrUpdateEmployeeDTO updateEmployee)
        {
            try
            {
                var Repo = _unitOfWork.GetRepoartory<Employee>();
                var Employee = await Repo.GetByIdAsync(id);
                if (Employee is null) { return false; }




                // ✅ تحقق من وجود المدير لو تم إدخاله
                if (updateEmployee.SupervisorId.HasValue && !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == updateEmployee.SupervisorId))
                    throw new ArgumentException("Supervisor does not exist.");

                // ✅ تحقق من وجود الدور لو تم إدخاله
                if (updateEmployee. floorsNumberWork.HasValue && !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(f => f.FloorsNumber == updateEmployee.floorsNumberWork))
                    throw new ArgumentException("Floor does not exist.");

                // ✅ تحقق من وجود الدور لو تم إدخاله
                if (updateEmployee.floorMangeNumber.HasValue && !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(f => f.FloorsNumber == updateEmployee.floorMangeNumber))
                    throw new ArgumentException("Floor does not exist.");


                //var Floor = _unitOfWork.GetRepoartory<Floors>().GetAllAsync(F => F.Id == updateEmployee.floorMangeNumber!.Value).Result.FirstOrDefault();
                //if (Floor is null) { return false; }

                //if (updateEmployee.floorMangeNumber.HasValue && !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(f => f.FloorsNumber == updateEmployee.floorMangeNumber))
                //    throw new ArgumentException("Floor does not exist.");






                _mapper.Map(updateEmployee, Employee);
                Repo.Update(Employee);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region DeleteAsync
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var Employee = await _unitOfWork.GetRepoartory<Employee>().GetByIdAsync(id);
                if (Employee is null) { return false; }

                // ✅ Business Rule: لا تحذف لو عنده مرؤوسين
                if (Employee.Subordinates.Any())
                    throw new InvalidOperationException("Cannot delete employee with subordinates.");

                _unitOfWork.GetRepoartory<Employee>().Remove(Employee);
                var IsRemoved = await _unitOfWork.SaveChangesAsync() > 0;
                return IsRemoved;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

    }
}
