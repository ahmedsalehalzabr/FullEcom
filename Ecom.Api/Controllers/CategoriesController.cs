using AutoMapper;
using Ecom.Core.Dto;
using Ecom.Core.Entites.Product;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{

    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> get()
        {
            try
            {
                var category= await work.CategoryRepositry.GetAllAsync();
                if (category is null)
                return BadRequest();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(error: ex.Message);
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getbyId(int id)
        {
            try
            {
                var categroy = await work.CategoryRepositry.GetByIdAsync(id);
                if(categroy is null)
                    return BadRequest();
                return Ok(categroy);
            }
            catch (Exception ex)
            {
                return BadRequest(error: ex.Message);
            }
        }

        [HttpPost("add-category")]
        public async Task<IActionResult> add(CategoryDto categoryDto)
        {
            try
            {
                //Maping
                var category = mapper.Map<Category>(categoryDto);
                await work.CategoryRepositry.AddAsync(category);
                return Ok(new {message="Item has been Added"});
            }
            catch (Exception ex)
            {
                return BadRequest(error: ex.Message);

            }
        }
        [HttpPut("update-category")]
        public async Task<IActionResult> update(UbdataCategoryDto ubdateCategoryDto)
        {
            try
            {
                var category = mapper.Map<Category>(ubdateCategoryDto);
                    await work.CategoryRepositry.UpdateAsync(category);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(error: ex.Message);

            }
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                await work.CategoryRepositry.DeleteAsync(id);
                return Ok(new {message="item has been deleted"});
            }
            catch (Exception ex)
            {
                return BadRequest(error: ex.Message);

            }
        }
    }
}
