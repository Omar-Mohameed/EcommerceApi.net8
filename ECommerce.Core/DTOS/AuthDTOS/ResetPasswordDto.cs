using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.DTOS.AuthDTOS
{
    public class ResetPasswordDto : LoginDto
    {
        public string Token { get; set; }
    }
}
