using ECommerce.Core.DTOS.AuthDTOS;
using ECommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDto registorDTO);
        Task<string> LoginAsync(LoginDto loginDTO);
        Task<bool> ActiveAccount(ActiveAccountDTO activeAccountDto);
        Task<bool> ResendConfirmation(string email);
        Task<Address> getUserAddress(string email);
        Task<bool> UpdateAddress(string email, Address address);
    }
}
