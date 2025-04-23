using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Dto;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpPost("Register")]
        public async Task<IActionResult> register(RegisterDto registerDto)
        {
            string result = await work.Auth.RegisterAsync(registerDto);
            if (result !="done")
            {
                return BadRequest(new ResponseAPI(400, result));
            }
            return Ok(new ResponseAPI(200,result));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDto loginDto)
        {
            var result = await work.Auth.LoginAsync(loginDto);
            Response.Cookies.Append("token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
            });
            return Ok(new ResponseAPI(200,result));
        }
    }
}
