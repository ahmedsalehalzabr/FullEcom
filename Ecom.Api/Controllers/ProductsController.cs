using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Dto;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> get()
        {
            try
            {
                var Product = await work.ProductRepositry
                    .GetAllAsync(x => x.Category,x => x.Photos);
                var result = mapper.Map<List<ProductDto>>(Product);
                if (Product is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(result);
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
    }
}
