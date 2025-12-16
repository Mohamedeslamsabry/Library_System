using Microsoft.AspNetCore.Mvc;
using Presentiton.Attribute;
using Service_Abstraction.Interfaces;
using Shared;
using Shared.DTO.Shelf;

namespace Presentiton
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShelfController(IShelfService _shelfService) : ControllerBase
    {
        #region Get All Shelf

        [HttpGet]
        [Cash]
        public async Task<ActionResult<PaginatedResult<ShelfDTO>>> GetAllShelves([FromQuery]ShelfQueryParamter shelfQuery)
        {
            var shelves = await _shelfService.GetAllAsync(shelfQuery);
            return Ok(shelves);
        }
        #endregion  

        #region Get Shelf By Id
        [HttpGet("{Id}")]
        [Cash]
        public async Task<ActionResult<ShelfDTO>> GetShelfById(int Id)
        {
            var shelf = await _shelfService.GetByIdAsync(Id);
            return Ok(shelf);
        }
        #endregion

        #region Create Shelf
        [HttpPost("Create")]
        public async Task<ActionResult<CreateOrUpdateShelfDTO>> CreateFloor(CreateOrUpdateShelfDTO CreateShelf)
        {
            var Shelf = await _shelfService.CreateAsync(CreateShelf);
            return Ok(Shelf);
        }
        #endregion

        #region Update Shelf
        [HttpPut("Update/{Id:int}")]
        public async Task<ActionResult<CreateOrUpdateShelfDTO>> UpdateFloor([FromRoute] int Id, CreateOrUpdateShelfDTO UpdateShelf)
        {
            var Shelf = await _shelfService.UpdateAsync(Id, UpdateShelf);
            return Ok(Shelf);
        }
        #endregion

        #region Delete Shelf

        [HttpDelete("Delete/{Id:int}")]
        public async Task<ActionResult<bool>> DeleteFloor([FromRoute] int Id)
        {
            var Shelf = await _shelfService.DeleteAsync(Id);
            return Ok(Shelf);
        }
        #endregion
    }
}
