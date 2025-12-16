using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.Entities.Product;

namespace Ecom.Core.Interfaces
{
    public interface ICategoryRepo : IGenericRepo<Category>
    {
        Task<ErrorOr<Category>> AddCategoryAsync(Category category);
        Task<ErrorOr<Category>> UpdateCategoryAsync(Category category);
        // check if category name is DuplicateName
        Task<bool> DuplicateName(string name);
    }
}
