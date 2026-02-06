using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Exceptions;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Users_Models;
using Microsoft.EntityFrameworkCore;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.User;

namespace Service_Implemention.Service
{
    public class UserService(IUnitOfWork _unitOfWork, IMapper _mapper) : IUserService
    {
        #region GetAllAsync

        public async Task<PaginatedResult<UserDTO>> GetAllAsync(UserQueryParamter userQuery)
        {
            var Specification = new UserSpecification(userQuery);
            var Users = await _unitOfWork.GetRepoartory<Users>().GetAllAsync(Specification);
            var UserDTO = _mapper.Map<IEnumerable<Users>, IEnumerable<UserDTO>>(Users);

            #region Paggention
            var spec = new UserCountSpecifcation(userQuery);
            var TotalCount = await _unitOfWork.GetRepoartory<Users>().CountAsync(spec);
            #endregion

            return new PaginatedResult<UserDTO>(TotalCount, Users.Count(), userQuery.PageIndex, UserDTO);
        }
        #endregion

        #region GetByIdAsync
        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var User = await _unitOfWork.GetRepoartory<Users>().GetByIdAsync(id);
            return User == null ? throw new UserNotFoundException(id) : _mapper.Map<UserDTO>(User);
        }
        #endregion

        #region CreateAsync
        public async Task<CreateUserResult> CreateAsync(CreateOrUpdateUserDTO createUser)
        {
            try
            {
                var usersRepo = _unitOfWork.GetRepoartory<Users>();

                var phoneExists = await usersRepo.AnyAsync(x => x.User_Phone == createUser.User_Phone);
                if (phoneExists)
                {
                    return new CreateUserResult
                    {
                        Success = false,
                        ErrorCode = "DuplicatePhone",
                        ErrorField = "User_Phone",
                        Message = "The phone number is already registered.."
                    };
                }

                var emailExists = await usersRepo.AnyAsync(x => x.User_Email == createUser.User_Email);
                if (emailExists)
                {
                    return new CreateUserResult
                    {
                        Success = false,
                        ErrorCode = "DuplicateEmail",
                        ErrorField = "User_Email",
                        Message = "The email address is already registered.."
                    };
                }

                if (createUser.EmployeeId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == createUser.EmployeeId))
                {
                    return new CreateUserResult
                    {
                        Success = false,
                        ErrorCode = "EmployeeNotFound",
                        ErrorField = "EmployeeId",
                        Message = "The associated employee does not exist."
                    };
                }

                var user = _mapper.Map<CreateOrUpdateUserDTO, Users>(createUser);

                await usersRepo.AddAsync(user);
                var saved = await _unitOfWork.SaveChangesAsync() > 0;

                if (!saved)
                {
                    return new CreateUserResult
                    {
                        Success = false,
                        ErrorCode = "SaveFailed",
                        Message = "An error occurred during saving.."
                    };
                }

                return new CreateUserResult
                {
                    Success = true,
                    Message = "User created successfully."
                };
            }
            catch (DbUpdateException)
            {
                return new CreateUserResult
                {
                    Success = false,
                    ErrorCode = "UniqueConstraintViolation",
                    Message = "There is duplicate data (phone or email)."
                };
            }
            catch (Exception)
            {
                return new CreateUserResult
                {
                    Success = false,
                    ErrorCode = "UnexpectedError",
                    Message = "An unexpected error occurred."
                };
            }
        }
        #endregion

        #region UpdateAsync
        public async Task<UpdateUserResult> UpdateAsync(int id, CreateOrUpdateUserDTO updateUser)
        {
            try
            {
                var usersRepo = _unitOfWork.GetRepoartory<Users>();
                var user = await usersRepo.GetByIdAsync(id);

                if (user is null)
                {
                    return new UpdateUserResult
                    {
                        Success = false,
                        ErrorCode = "NotFound",
                        Message = "User not found."
                    };
                }

                if (updateUser.EmployeeId.HasValue &&
                    !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(e => e.Id == updateUser.EmployeeId))
                {
                    return new UpdateUserResult
                    {
                        Success = false,
                        ErrorCode = "EmployeeNotFound",
                        ErrorField = "EmployeeId",
                        Message = "The associated employee does not exist."
                    };
                }

                if (!string.Equals(user.User_Phone, updateUser.User_Phone, StringComparison.Ordinal))
                {
                    var phoneExists = await usersRepo.AnyAsync(x => x.User_Phone == updateUser.User_Phone && x.Id != id);
                    if (phoneExists)
                    {
                        return new UpdateUserResult
                        {
                            Success = false,
                            ErrorCode = "DuplicatePhone",
                            ErrorField = "User_Phone",
                            Message = "The phone number is already registered."
                        };
                    }
                }

                if (!string.Equals(user.User_Email, updateUser.User_Email, StringComparison.OrdinalIgnoreCase))
                {
                    var emailExists = await usersRepo.AnyAsync(x => x.User_Email == updateUser.User_Email && x.Id != id);
                    if (emailExists)
                    {
                        return new UpdateUserResult
                        {
                            Success = false,
                            ErrorCode = "DuplicateEmail",
                            ErrorField = "User_Email",
                            Message = "The email address is already registered."
                        };
                    }
                }

                _mapper.Map(updateUser, user);

                usersRepo.Update(user);
                var saved = await _unitOfWork.SaveChangesAsync() > 0;

                if (!saved)
                {
                    return new UpdateUserResult
                    {
                        Success = false,
                        ErrorCode = "SaveFailed",
                        Message = "No changes were saved."
                    };
                }

                return new UpdateUserResult
                {
                    Success = true,
                    Message = "User data was successfully updated."
                };
            }
            catch (DbUpdateException)
            {
                return new UpdateUserResult
                {
                    Success = false,
                    ErrorCode = "UniqueConstraintViolation",
                    Message = "There is duplicate data (phone or email)."
                };
            }
            catch (Exception)
            {
                return new UpdateUserResult
                {
                    Success = false,
                    ErrorCode = "UnexpectedError",
                    Message = "An unexpected error occurred."
                };
            }
        }

        #endregion
    }
}
