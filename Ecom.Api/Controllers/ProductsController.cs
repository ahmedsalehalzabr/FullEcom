using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Dto;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IImageManagementService service;
        public ProductsController(IUnitOfWork work, IMapper mapper, IImageManagementService service) : base(work, mapper)
        {
            this.service = service;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> get(string sort)
        {
            try
            {
                var Product = await work.ProductRepositry
                    .GetAllAsync(sort);
                
                return Ok(Product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getbyid(int id)
        {
            try
            {
                var Product = await work.ProductRepositry.GetByIdAsync(id,
                    x => x.Category,x => x.Photos);
                var result =  mapper.Map<ProductDto>(Product);
                if (Product is null)
                {
                    return BadRequest(new ResponseAPI(400, "Item Not Found"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Add-Product")]
        public async Task<IActionResult> Add(AddProductDto addProductDto)
        {
            try
            {
                await work.ProductRepositry.AddAsync(addProductDto);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [HttpPut("Update-Product")]
        public async Task<IActionResult> Update(UpdateProudactDto updateProudactDto)
        {
            try 
            {
                await work.ProductRepositry.UpdateAsync(updateProudactDto);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpDelete("Delete-Product/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var product = await work.ProductRepositry.GetByIdAsync(Id, x => x.Photos, x => x.Category);

                await work.ProductRepositry.DeleteAsync(product);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

    }
}
