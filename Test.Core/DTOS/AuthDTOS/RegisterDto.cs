using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.DTOS.AuthDTOS
{
    public class RegisterDto : LoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
