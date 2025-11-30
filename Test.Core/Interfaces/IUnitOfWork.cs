using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepo Categories { get; }
        IProductRepo Products { get; }
        ICustomerBasketRepo Baskets { get; }
        Task<int> SaveChangesAsync();
    }
}
