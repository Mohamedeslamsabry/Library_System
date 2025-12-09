using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Floors_Models;
using Domain_Layer.Models.Users_Models;
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
            return User == null ? null : _mapper.Map<UserDTO>(User);
        }
        #endregion

        #region CreateAsync
        public async Task<bool> CreateAsync(CreateOrUpdateUserDTO CreateUser)
        {
            try
            {
                bool EmailIsExist = _unitOfWork.GetRepoartory<Users>().GetAllAsync(X => X.User_Email == CreateUser.User_Email).Result.Any();
                bool PhoneIsExist = _unitOfWork.GetRepoartory<Users>().GetAllAsync(X => X.User_Phone == CreateUser.User_Phone).Result.Any();
                if (EmailIsExist || PhoneIsExist)
                {
                    return false;
                }
                var User = _mapper.Map<CreateOrUpdateUserDTO, Users>(CreateUser);


                if (CreateUser.EmployeeId.HasValue && !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(f => f.Id == CreateUser.EmployeeId))
                    throw new ArgumentException("Employee does not exist.");

                await _unitOfWork.GetRepoartory<Users>().AddAsync(User);
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
        public async Task<bool> UpdateAsync(int id, CreateOrUpdateUserDTO UpdateUser)
        {
            try
            {

                bool EmailIsExist = _unitOfWork.GetRepoartory<Users>().GetAllAsync(X => X.User_Email == UpdateUser.User_Email && X.Id != id).Result.Any();
                bool PhoneIsExist = _unitOfWork.GetRepoartory<Users>().GetAllAsync(X => X.User_Phone == UpdateUser.User_Phone && X.Id != id).Result.Any();
                if (EmailIsExist || PhoneIsExist)
                {
                    return false;
                }

                var Repo = _unitOfWork.GetRepoartory<Users>();
                var User = await Repo.GetByIdAsync(id);
                if (User is null) { return false; }

                // ✅ تحقق من وجود الدور لو تم إدخاله
                if (UpdateUser.EmployeeId.HasValue && !await _unitOfWork.GetRepoartory<Employee>().AnyAsync(f => f.Id == UpdateUser.EmployeeId))
                    throw new ArgumentException("Employee does not exist.");

                _mapper.Map(UpdateUser, User);
                Repo.Update(User);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #endregion
    }
}
