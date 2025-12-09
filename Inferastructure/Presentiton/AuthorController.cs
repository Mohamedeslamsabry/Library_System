using Microsoft.AspNetCore.Mvc;
using Service_Abstraction.Interfaces;
using Shared;
using Shared.DTO.authors;

namespace Presentiton
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController(IAuthorsService _authorsService) : ControllerBase
    {
        #region Get All Author
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAllAuthor([FromQuery] AuthorsQueryParamter authorsQuery)
        {
            var Authors = await _authorsService.GetAllAsync(authorsQuery);
            return Ok(Authors);
        }
        #endregion  

        #region Get Author By Id
        [HttpGet("{Id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthorById(int Id)
        {
            var Author = await _authorsService.GetByIdAsync(Id);
            return Ok(Author);
        }
        #endregion

        #region Create Author
        [HttpPost("Create")]
        public async Task<ActionResult<CreateOrUpdateAuthorDTO>> CreateAuthor(CreateOrUpdateAuthorDTO createAuthor)
        {
            var Author = await _authorsService.CreateAsync(createAuthor);
            return Ok(Author);
        }
        #endregion

        #region Update Author
        [HttpPut("Update/{Id:int}")]
        public async Task<ActionResult<CreateOrUpdateAuthorDTO>> UpdateAuthor([FromRoute] int Id, CreateOrUpdateAuthorDTO UpdateAuthor)
        {
            var Author = await _authorsService.UpdateAsync(Id, UpdateAuthor);
            return Ok(Author);
        }
        #endregion
    }
}
