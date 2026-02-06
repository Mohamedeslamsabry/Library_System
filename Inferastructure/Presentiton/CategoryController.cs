using Microsoft.AspNetCore.Mvc;
using Presentiton.Attribute;
using Service_Abstraction.Interfaces;
using Shared;
using Shared.DTO.Employee;

namespace Presentiton
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoriService categoriService) : ControllerBase
    {
        #region Get All Category

        [HttpGet]
        [Cash]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllcategory([FromQuery] CategorieQueryParamter categorieQuery)
        {
            var categories = await categoriService.GetAllAsync(categorieQuery);
            return Ok(categories);
        }
        #endregion  
    }
}
