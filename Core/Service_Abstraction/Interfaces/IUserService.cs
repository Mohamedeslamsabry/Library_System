using Shared;
using Shared.DTO.User;

namespace Service_Abstraction.Interfaces
{
    public interface IUserService
    {
        #region GetAllAsync
        Task<PaginatedResult<UserDTO>> GetAllAsync(UserQueryParamter userQuery);
        #endregion

        #region GetByIdAsync
        Task<UserDTO?> GetByIdAsync(int id);
        #endregion

        #region CreateAsync
        Task<CreateUserResult> CreateAsync(CreateOrUpdateUserDTO createUser);
        #endregion

        #region UpdateAsync
        Task<UpdateUserResult> UpdateAsync(int id, CreateOrUpdateUserDTO updateUser);
        #endregion
    }
}
