using Microsoft.AspNetCore.Mvc;
using Service_Abstraction.Interfaces;
using Shared;
using Shared.DTO.Publisher;

namespace Presentiton
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController(IPublisherService _publisherService) : ControllerBase
    {
        #region Get All Publisher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDTO>>> GetAllPublisher([FromQuery] PublisherQueryParamter publisherQuery)
        {
            var Publisher = await _publisherService.GetAllAsync(publisherQuery);
            return Ok(Publisher);
        }
        #endregion  

        #region Get Publisher By Id
        [HttpGet("{Id}")]
        public async Task<ActionResult<PublisherDTO>> GetPublisherById(int Id)
        {
            var Publisher = await _publisherService.GetByIdAsync(Id);
            return Ok(Publisher);
        }
        #endregion

        #region Create Publisher
        [HttpPost("Create")]
        public async Task<ActionResult<CreateOrUpdatePublisherDTO>> CreatePublisher(CreateOrUpdatePublisherDTO CreatePublisher)
        {
            var Publisher = await _publisherService.CreateAsync(CreatePublisher);
            return Ok(Publisher);
        }
        #endregion

        #region Update Publisher
        [HttpPut("Update/{Id:int}")]
        public async Task<ActionResult<CreateOrUpdatePublisherDTO>> UpdatePublisher([FromRoute] int Id, CreateOrUpdatePublisherDTO UpdatePubliser)
        {
            var Publisher = await _publisherService.UpdateAsync(Id, UpdatePubliser);
            return Ok(Publisher);
        }
        #endregion

        #region Delete Publisher

        [HttpDelete("Delete/{Id:int}")]
        public async Task<ActionResult<bool>> DeletePublisher([FromRoute] int Id)
        {
            var Publisher = await _publisherService.DeleteAsync(Id);
            return Ok(Publisher);
        }
        #endregion
    }
}
