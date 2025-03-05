using AutoMapper;
using Ecom.Api.Helper;
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
                return BadRequest(new ResponseAPI(400));
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
                    return BadRequest(new ResponseAPI(400,"Item Not Found"));
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
                return Ok(new ResponseAPI(200,"Item has been Added"));
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
                return Ok(new ResponseAPI(200, "Item has been updated"));
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
                return Ok(new ResponseAPI(200,"item has been deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(error: ex.Message);

            }
        }
    }
}
