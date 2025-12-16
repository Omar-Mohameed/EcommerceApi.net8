using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.DTOS.AuthDTOS;

namespace Test.Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto emailDTO);
    }
}
