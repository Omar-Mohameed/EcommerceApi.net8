using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Entities.Product;

namespace Test.Core.Interfaces
{
    public interface ICategoryRepo : IGenericRepo<Category>
    {
        Task<ErrorOr<Category>> AddAsync(Category category);
        // check if category name is DuplicateName
        Task<bool> DuplicateName(string name);
    }
}
