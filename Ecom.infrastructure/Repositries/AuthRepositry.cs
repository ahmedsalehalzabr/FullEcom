using Ecom.Core.Dto;
using Ecom.Core.Entites;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
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
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;

        public AuthRepositry(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
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
            string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            SendEmail(user.Email, code, "active", "ActiveEmail", "Please active your email");
            return "done";
        }
        public async Task SendEmail(string email, string code,string component,string subject,string messge)
        {
            var result = new EmailDto(
                email, "ahmedalzabr1@gmail.com",
                subject, EmailStringBody.send(email, code, component, messge));
            await emailService.SendEmail(result);
        }
    }
}
