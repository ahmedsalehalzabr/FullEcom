using Ecom.Core.Dto;
using Ecom.Core.Entites;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries
{
    public class AuthRepositry:IAuth
    {
        private readonly UserManager<AppUser> userManager;

        public AuthRepositry(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            if(registerDto == null)
            {
                return null;
            }
            if(await userManager.FindByNameAsync(registerDto.UserName) is not null) 
            {
                return "this Username is already registered";
            }
            if (await userManager.FindByEmailAsync(registerDto.Email) is not null)
            {
                return "this Email is already registered";
            }
            AppUser user = new AppUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
            };
            var result = await userManager.CreateAsync(user,registerDto.Password);
            if(result.Succeeded is not true)
            {
                return result.Errors.ToList()[0].Description;
            }
            return "done";
        }
    }
}
