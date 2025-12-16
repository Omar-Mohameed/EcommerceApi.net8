using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Services.Errors
{
    public class ProductErrors
    {
        public static Error NotFound(int id) => Error.NotFound(
            code: "Product.NotFound",
            description: $"Product with Id {id} was not found."
        );
        public static Error InvalidName => Error.Validation(
            code: "Category.InvalidName",
            description: "Category name must be between 3 and 50 characters."
        );
        public static Error InvalidPhoto => Error.Validation(
            code: "Product.Photos.Empty",
            description: "Product must contain at least one image."
        );
    }
}
