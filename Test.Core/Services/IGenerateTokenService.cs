using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.Entities;

namespace Ecom.Core.Services
{
    public interface IGenerateTokenService
    {
        string GetAndCreateToken(AppUser user);
    }
}
