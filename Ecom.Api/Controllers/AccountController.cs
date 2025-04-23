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
            if (result.StartsWith("please"))
            {
                return BadRequest(new ResponseAPI(400,result));
            }
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
        [HttpPost("active-account")]
        public async Task<IActionResult> active(ActiveAccountDto accountDto)
        {
            var result = await work.Auth.ActiveAccount(accountDto);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(200));
        }
        [HttpPost("send-email-forget-password")]
        public async Task<IActionResult> forget(string email)
        {
            var result = await work.Auth.SendEmailForForgetPassword(email);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(200));
        }
    }
}
