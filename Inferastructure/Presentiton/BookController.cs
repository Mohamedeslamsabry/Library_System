using Microsoft.AspNetCore.Mvc;
using Service_Abstraction.Interfaces;
using Shared;
using Shared.DTO.Book;

namespace Presentiton
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController(IBookService _bookService) : ControllerBase
    {
        #region Get All Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBook([FromQuery] BookQueryPartmer bookQuery)
        {
            var Books = await _bookService.GetAllAsync(bookQuery);
            return Ok(Books);
        }
        #endregion  

        #region Get Book By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(int id)
        {
            var Book = await _bookService.GetByIdAsync(id);
            return Ok(Book);
        }
        #endregion

        #region Create Book
        [HttpPost("Create")]
        public async Task<ActionResult<CreateOrUpdateBookDto>> CreateBook([FromBody] CreateOrUpdateBookDto createBook)
        {
            var Book = await _bookService.CreateAsync(createBook);
            return Ok(Book);
        }
        #endregion

        #region Update Employee
        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<CreateOrUpdateBookDto>> UpdateBook([FromRoute] int id, CreateOrUpdateBookDto updateBook)
        {
            var Book = await _bookService.UpdateAsync(id, updateBook);
            return Ok(Book);
        }
        #endregion

        #region DeleteEmployee

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> DeleteBook([FromRoute] int id)
        {
            var Book = await _bookService.DeleteAsync(id);
            return Ok(Book);
        }
        #endregion
    }
}
