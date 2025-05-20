using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Dto;
using Ecom.Core.interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Ecom.Core.Entites;

namespace Ecom.Api.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpPut("update-address")]
        public async Task<IActionResult> updateAddress(ShipAddressDto shipAddressDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found in claims.");

            var address = mapper.Map<Address>(shipAddressDto);
            var result = await work.Auth.UpdateAddress(email, address);
            return result ? Ok() : BadRequest();
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


        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> forget(string email)
        {
            var result = await work.Auth.SendEmailForForgetPassword(email);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(200));
        }






        [HttpPost("reset-password")]
        public async Task<IActionResult> reset(RestPasswordDto restPasswordDTO)
        {
            var result = await work.Auth.ResetPassword(restPasswordDTO);
            if (result == "done")
            {
                return Ok(new ResponseAPI(200));
            }
            return BadRequest(new ResponseAPI(400));
        }
    }
}
