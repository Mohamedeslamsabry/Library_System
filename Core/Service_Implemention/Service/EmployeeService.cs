using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Exceptions;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Floors_Models;
using Microsoft.EntityFrameworkCore;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.Employee;
using Shared.Error;

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
            #region  Include By Specification
            //var Specification = new EmployeeSpecifcation(id);
            //var Employee = await _unitOfWork.GetRepoartory<Employee>().GetByIdAsync(Specification);
            //if (Employee is null)
            //{
            //    return null!;
            //}
            //return _mapper.Map<EmployeeDTO>(Employee);
            #endregion
            var employee = await _unitOfWork.GetRepoartory<Employee>().GetByIdAsync(id);
            return employee == null ? throw new EmployeeNotFoundException(id) : _mapper.Map<EmployeeDTO>(employee);
        }
        #endregion

        #region CreateAsync

        public async Task<CreateEmployeeResult> CreateAsync(CreateOrUpdateEmployeeDTO createEmployee)
        {
            try
            {
                var repo = _unitOfWork.GetRepoartory<Employee>();

                var phoneExists = await repo.AnyAsync(x => x.PhoneNumber == createEmployee.PhoneNumber);
                if (phoneExists)
                {
                    return new CreateEmployeeResult
                    {
                        Success = false,
                        ErrorCode = "DuplicatePhone",
                        ErrorField = "PhoneNumber",
                        Message = "The phone number is already registered."
                    };
                }

                var emailExists = await repo.AnyAsync(x => x.Email == createEmployee.Email);
                if (emailExists)
                {
                    return new CreateEmployeeResult
                    {
                        Success = false,
                        ErrorCode = "DuplicateEmail",
                        ErrorField = "Email",
                        Message = "The email address is already registered."
                    };
                }

                if (createEmployee.SupervisorId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == createEmployee.SupervisorId))
                {
                    return new CreateEmployeeResult
                    {
                        Success = false,
                        ErrorCode = "SupervisorNotFound",
                        ErrorField = "SupervisorId",
                        Message = "Supervisor NotFound."
                    };
                }

                if (createEmployee.floorsNumberWork.HasValue &&
                    !await _unitOfWork.GetRepoartory<Floors>().AnyAsync(f => f.Id == createEmployee.floorsNumberWork))
                {
                    return new CreateEmployeeResult
                    {
                        Success = false,
                        ErrorCode = "FloorNotFound",
                        ErrorField = "floorsNumberWork",
                        Message = "The floor does not exist."
                    };
                }

                var employee = _mapper.Map<CreateOrUpdateEmployeeDTO, Employee>(createEmployee);

                await repo.AddAsync(employee);
                var saved = await _unitOfWork.SaveChangesAsync() > 0;

                if (!saved)
                {
                    return new CreateEmployeeResult
                    {
                        Success = false,
                        ErrorCode = "SaveFailed",
                        Message = "An error occurred during saving.."
                    };
                }

                return new CreateEmployeeResult
                {
                    Success = true,
                    Message = "Succesfully."
                };
            }
            catch (DbUpdateException) 
            {
                return new CreateEmployeeResult
                {
                    Success = false,
                    ErrorCode = "UniqueConstraintViolation",
                    Message = "There is duplicate data (phone or email)."
                };
            }
            catch (Exception)
            {
                return new CreateEmployeeResult
                {
                    Success = false,
                    ErrorCode = "UnexpectedError",
                    Message = "An unexpected error occurred."
                };
            }
        }
        #endregion

        #region UpdateAsync
        //public async Task<bool> UpdateAsync(int id, CreateOrUpdateEmployeeDTO updateEmployee)
        //{
        //    try
        //    {

        //        bool EmailIsExist = _unitOfWork.GetRepoartory<Employee>().GetAllAsync(X => X.Email == updateEmployee.Email && X.Id != id).Result.Any();
        //        bool PhoneIsExist = _unitOfWork.GetRepoartory<Employee>().GetAllAsync(X => X.PhoneNumber == updateEmployee.PhoneNumber && X.Id != id).Result.Any();
        //        if (EmailIsExist || PhoneIsExist)
        //        {
        //            return false;
        //        }

        //        var Repo = _unitOfWork.GetRepoartory<Employee>();
        //        var Employee = await Repo.GetByIdAsync(id);
        //        if (Employee is null) { return false; }

        //        // ✅ تحقق من وجود المدير لو تم إدخاله
        //        if (updateEmployee.SupervisorId.HasValue && !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == updateEmployee.SupervisorId))
        //            throw new ArgumentException("Supervisor does not exist.");

        //        // ✅ تحقق من وجود الدور لو تم إدخاله
        //        if (updateEmployee.floorsNumberWork.HasValue && !await _unitOfWork.GetRepoartory<Floors>().AnyAsync(f => f.Id == updateEmployee.floorsNumberWork))
        //            throw new ArgumentException("Floor does not exist.");


        //        _mapper.Map(updateEmployee, Employee);
        //        Repo.Update(Employee);
        //        return await _unitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public async Task<UpdateEmployeeResult> UpdateAsync(int id, CreateOrUpdateEmployeeDTO updateEmployee)
        {
            try
            {
                var repo = _unitOfWork.GetRepoartory<Employee>();
                var employee = await repo.GetByIdAsync(id);

                if (employee is null)
                {
                    return new UpdateEmployeeResult
                    {
                        Success = false,
                        ErrorCode = "NotFound",
                        Message = "Employee Not Found."
                    };
                }


                if (updateEmployee.SupervisorId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == updateEmployee.SupervisorId))
                {
                    return new UpdateEmployeeResult
                    {
                        Success = false,
                        ErrorCode = "SupervisorNotFound",
                        ErrorField = "SupervisorId",
                        Message = "Supervisor NotFound."
                    };
                }

                if (updateEmployee.floorsNumberWork.HasValue &&
                    !await _unitOfWork.GetRepoartory<Floors>().AnyAsync(f => f.Id == updateEmployee.floorsNumberWork))
                {
                    return new UpdateEmployeeResult
                    {
                        Success = false,
                        ErrorCode = "FloorNotFound",
                        ErrorField = "floorsNumberWork",
                        Message = "Floor NotFound."
                    };
                }

                if (!string.Equals(employee.PhoneNumber, updateEmployee.PhoneNumber, StringComparison.OrdinalIgnoreCase))
                {
                    var phoneExists = await repo.AnyAsync(x => x.PhoneNumber == updateEmployee.PhoneNumber && x.Id != id);
                    if (phoneExists)
                    {
                        return new UpdateEmployeeResult
                        {
                            Success = false,
                            ErrorCode = "DuplicatePhone",
                            ErrorField = "PhoneNumber",
                            Message = "The phone number is already registered.."
                        };
                    }
                }

                if (!string.Equals(employee.Email, updateEmployee.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var emailExists = await repo.AnyAsync(x => x.Email == updateEmployee.Email && x.Id != id);
                    if (emailExists)
                    {
                        return new UpdateEmployeeResult
                        {
                            Success = false,
                            ErrorCode = "DuplicateEmail",
                            ErrorField = "Email",
                            Message = "The email address is already registered.."
                        };
                    }
                }

                _mapper.Map(updateEmployee, employee);

                repo.Update(employee);

                var saved = await _unitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                {
                    return new UpdateEmployeeResult
                    {
                        Success = false,
                        ErrorCode = "SaveFailed",
                        Message = "No changes were saved.."
                    };
                }

                return new UpdateEmployeeResult
                {
                    Success = true,
                    Message = "The employee data has been successfully updated.."
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new UpdateEmployeeResult
                {
                    Success = false,
                    ErrorCode = "ConcurrencyConflict",
                    Message = "A conflict occurred during the update. Please try again after updating the data.."
                };
            }
            catch (DbUpdateException ex)
            {

                Console.WriteLine(ex.Message);
                return new UpdateEmployeeResult
                {
                    Success = false,
                    ErrorCode = "UniqueConstraintViolation",
                    Message = "There is duplicate data (phone or email)."
                };
            }
            catch (Exception)
            {
                return new UpdateEmployeeResult
                {
                    Success = false,
                    ErrorCode = "UnexpectedError",
                    Message = "An unexpected error occurred."
                };
            }
        }
        #endregion

        #region DeleteAsync
        //public async Task<bool> DeleteAsync(int id)
        //{
        //    try
        //    {
        //        var Employee = await _unitOfWork.GetRepoartory<Employee>().GetByIdAsync(id);
        //        if (Employee is null) { return false; }

        //        // ✅ Business Rule: Subordinates
        //        if (Employee.Subordinates.Any())
        //        {
        //            foreach (var employee in Employee.Subordinates)
        //            {
        //                employee.SupervisorId = null;
        //            }
        //        }

        //        // ✅ Business Rule: Users
        //        if (Employee.Users.Any())
        //        {
        //            foreach (var User in Employee.Users)
        //            {
        //                User.EmployeeId = null;
        //            }
        //        }

        //        //// ✅ Business Rule: Floor

        //        if (Employee.FloorsMange is not null)
        //        {
        //            if (Employee.FloorsMange.EmployeeMangeId != null)
        //            {
        //                if (Employee.FloorsMange!.EmployeeMangeId == Employee.Id)
        //                {
        //                    Employee.FloorsMange.EmployeeMangeId = null;
        //                }
        //            }
        //        }

        //        // ✅ Business Rule: Borrows
        //        if (Employee.Borrows.Any())
        //        {
        //            foreach (var emp in Employee.Borrows)
        //            {
        //                emp.EmployeeId = null;
        //            }
        //        }
        //        _unitOfWork.GetRepoartory<Employee>().Remove(Employee);
        //        var IsRemoved = await _unitOfWork.SaveChangesAsync() > 0;
        //        return IsRemoved;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}

        public async Task<Result<int>> DeleteAsync(int id)
        {
            try
            {
                var employeeRepo = _unitOfWork.GetRepoartory<Employee>();
                var employee = await employeeRepo.GetByIdAsync(id);

                if (employee is null)
                    return Result<int>.Fail("Employee not found.", ErrorCodes.EmployeeNotFound);

                if (employee.Subordinates?.Any() == true)
                {
                    foreach (var sub in employee.Subordinates)
                        sub.SupervisorId = null;
                }

                if (employee.Users?.Any() == true)
                {
                    foreach (var user in employee.Users)
                        user.EmployeeId = null;
                }

                if (employee.FloorsMange is not null)
                {
                    if (employee.FloorsMange.EmployeeMangeId != null &&
                        employee.FloorsMange.EmployeeMangeId == employee.Id)
                    {
                        employee.FloorsMange.EmployeeMangeId = null;
                    }
                }

                if (employee.Borrows?.Any() == true)
                {
                    foreach (var borrow in employee.Borrows)
                        borrow.EmployeeId = null;
                }

                employeeRepo.Remove(employee);

                var saved = await _unitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed, id);

                return Result<int>.Ok(id, "Employee deleted successfully.");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("Operation was canceled.", ErrorCodes.Canceled);
            }
            catch (DbUpdateException)
            {
                return Result<int>.Fail("Database update failed during delete.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("Unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }
        #endregion
    }
}
