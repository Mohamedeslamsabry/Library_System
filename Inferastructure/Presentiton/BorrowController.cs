using Microsoft.AspNetCore.Mvc;
using Service_Abstraction.Interfaces;
using Shared;
using Shared.DTO.Borrow;

namespace Presentiton
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowController(IBorrowService _borrowService) : ControllerBase
    {
        #region Get All Borrow
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowDTO>>> GetAllBorrow([FromQuery] BorrowQueryParamter borrowQuery)
        {
            var Borrows = await _borrowService.GetAllAsync(borrowQuery);
            return Ok(Borrows);
        }
        #endregion

        #region Get Borrow By Id

        [HttpGet("{userId:int}/{bookId:int}/{dateBorrow}")]
        public async Task<ActionResult<BorrowDTO>> GetBorrow(
            int userId, int bookId, DateTime dateBorrow)
        {
            var dto = await _borrowService.GetByIdAsync(userId, bookId, dateBorrow);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        #endregion

        #region Create Borrow
        [HttpPost("Create")]
        public async Task<ActionResult<CreateOrUpdateBorrowDTO>> CreateBorrow([FromBody] CreateOrUpdateBorrowDTO createBorrow)
        {
            var Borrow = await _borrowService.CreateAsync(createBorrow);
            return Ok(Borrow);
        }
        #endregion

        #region Update Borrow
        [HttpPut("Update/{oldUserId:int}/{oldBookId:int}/{oldDateBorrow}")]
        public async Task<IActionResult> UpdateBorrow(
            int oldUserId, int oldBookId, DateTime oldDateBorrow,
            [FromBody] CreateOrUpdateBorrowDTO newBorrow)
        {
            var Borrow = await _borrowService.UpdateByKeyAsync(oldUserId, oldBookId, oldDateBorrow, newBorrow);
            return Ok(Borrow);
        }

        #endregion

        #region Delete Borrow

        [HttpDelete("Delete/{userId:int}/{bookId:int}/{dateBorrow}")]
        public async Task<IActionResult> DeleteBorrow(int userId, int bookId, DateTime dateBorrow)
        {
            var Borrow = await _borrowService.DeleteByKeyAsync(userId, bookId, dateBorrow);
            return Ok(Borrow);
        }

        #endregion
    }
}
