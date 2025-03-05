﻿using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
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
                if (Product == null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(Product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
