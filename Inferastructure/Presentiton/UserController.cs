using Microsoft.AspNetCore.Mvc;
using Service_Abstraction.Interfaces;
using Shared;
using Shared.DTO.User;

namespace Presentiton
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        #region Get All User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUser([FromQuery] UserQueryParamter userQuery)
        {
            var Users = await userService.GetAllAsync(userQuery);
            return Ok(Users);
        }
        #endregion  

        #region Get User By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var User = await userService.GetByIdAsync(id);
            return Ok(User);
        }
        #endregion

        #region Create User
        [HttpPost("Create")]
        public async Task<ActionResult<CreateOrUpdateUserDTO>> CreateUser([FromBody] CreateOrUpdateUserDTO createUser)
        {
            var User = await userService.CreateAsync(createUser);
            return Ok(User);
        }
        #endregion

        #region Update User
        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<CreateOrUpdateUserDTO>> UpdateUser([FromRoute] int id, CreateOrUpdateUserDTO UpdateUser)
        {
            var User = await userService.UpdateAsync(id, UpdateUser);
            return Ok(User);
        }
        #endregion
    }
}
