using Microsoft.AspNetCore.Mvc;
using Presentiton.Attribute;
using Service_Abstraction.Interfaces;
using Shared.DTO.Floor;

namespace Presentiton
{
    [ApiController]
    [Route("api/[controller]")]
    public class FloorController(IFloorService _floorService) : ControllerBase
    {
        #region Get All Floor

        [HttpGet]
        [Cash]
        public async Task<ActionResult<IEnumerable<FloorDTO>>> GetAllFloor()
        {
            var Floors = await _floorService.GetAllAsync();
            return Ok(Floors);
        }
        #endregion  

        #region Get Floor By Id
        [HttpGet("{FloorNumber}")]
        [Cash]
        public async Task<ActionResult<FloorDTO>> GetShelfById(int FloorNumber)
        {
            var Floor = await _floorService.GetByIdAsync(FloorNumber);
            return Ok(Floor);
        }
        #endregion

        #region Create Floor
        [HttpPost("Create")]
        public async Task<ActionResult<CreateOrUpdateFloorDTO>> CreateShelf([FromBody] CreateOrUpdateFloorDTO CreateFloorDto)
        {
            var Floor = await _floorService.CreateAsync(CreateFloorDto);
            return Ok(Floor);
        }
        #endregion

        #region Update Floor
        [HttpPut("Update/{FloorNumber:int}")]
        public async Task<ActionResult<CreateOrUpdateFloorDTO>> UpdateFloor([FromRoute] int FloorNumber, CreateOrUpdateFloorDTO updateFloorDTO)
        {
            var Floor = await _floorService.UpdateAsync(FloorNumber, updateFloorDTO);
            return Ok(Floor);
        }
        #endregion

        #region Delete Floor

        [HttpDelete("Delete/{FloorNumber:int}")]
        public async Task<ActionResult<bool>> DeleteFloor([FromRoute] int FloorNumber)
        {
            var Floor = await _floorService.DeleteAsync(FloorNumber);
            return Ok(Floor);
        }
        #endregion
    }
}
