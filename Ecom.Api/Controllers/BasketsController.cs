using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Entites;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{

    public class BasketsController : BaseController
    {
        public BasketsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> get(string id)
        {
            var result = await work.CustomerBasketRepositry.GetBasketAsync(id);
            if (result == null)
            {
                return Ok(new CustomerBasket());
            }
            return Ok(result);
        }
        [HttpPost("update-basket")]
        public async Task<IActionResult> add(CustomerBasket customerBasket)
        {
            var _basket = await work.CustomerBasketRepositry.UpdateBasketAsync(customerBasket);
            return Ok(_basket);
        }
        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> delete(string id)
        {
           var result =  await work.CustomerBasketRepositry.DeleteBasketAsync(id);
            return result ? Ok(new ResponseAPI(200,"item delete")) : BadRequest(new ResponseAPI(400));
        }
    }
}
