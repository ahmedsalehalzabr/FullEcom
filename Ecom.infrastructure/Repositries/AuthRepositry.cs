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
        private readonly IGenerateToken generateToken;

        public AuthRepositry(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
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
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            SendEmail(user.Email, token, "active", "ActiveEmail", "Please active your email, click on button to active");
            return "done";
        }
        public async Task SendEmail(string email, string code,string component,string subject,string messge)
        {
            var result = new EmailDto(
                email, "ahmedalzabr1@gmail.com",
                subject, EmailStringBody.send(email, code, component, messge));
            await emailService.SendEmail(result);
        }

        public async Task<string> LoginAsync(LoginDto login)
        {
            if (login == null)
            {
                return null;
            }
            var finduser=await userManager.FindByEmailAsync(login.Email);
            if (!finduser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(finduser);
                await SendEmail(finduser.Email, token, "active", "ActiveEmail", "Please active your email, click on button to active");
                return "Please confirem your email first, we have send activat to your E-mail";
            }
            var result = await signInManager.CheckPasswordSignInAsync(finduser, login.Password,true);
            if (!result.Succeeded) 
            {
                return generateToken.GetAndCreateTokenAsync(finduser);
            }
            return "Please check your email and password, something went wrong";
        }
        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser is null)
            {
                return false;
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "Reset-Password", "Rest Password", "Click on button to Reset your password");
            return true;


        }
    }
}
