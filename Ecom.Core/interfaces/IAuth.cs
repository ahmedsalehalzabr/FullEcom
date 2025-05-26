using Ecom.Core.Dto;
using Ecom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto login);
        Task<bool> SendEmailForForgetPassword(string email);
        Task<string> ResetPassword(RestPasswordDto restPassword);
        Task<bool> ActiveAccount(ActiveAccountDto accountDto);
        Task<bool> UpdateAddress(string email, Address address);
        Task<Address> GetUserAddress(string email);
        
    }
}
