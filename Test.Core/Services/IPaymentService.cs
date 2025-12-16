using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Entities;

namespace Test.Core.Services
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketid, int? delivertMethodId);
        Task<string> CreateCheckoutSessionAsync(string basketId, int? deliveryMethodId);
    }
}
