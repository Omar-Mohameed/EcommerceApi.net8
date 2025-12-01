using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.DTOS.AuthDTOS;

namespace Test.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDto registorDTO);
        Task<string> LoginAsync(LoginDto loginDTO);
        Task<bool> ActiveAccount(ActiveAccountDTO activeAccountDto);
        Task<bool> ResendConfirmation(string email);
    }
}
