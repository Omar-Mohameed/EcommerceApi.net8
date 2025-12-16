using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Errors
{
    public static class CategoryErrors
    {
        public static Error NotFound(int id) => Error.NotFound(
            code: "Category.NotFound",
            description: $"Category with Id {id} was not found."
        );

        public static Error InvalidName => Error.Validation(
            code: "Category.InvalidName",
            description: "Category name must be between 3 and 50 characters."
        );
        public static Error DuplicateName(string name) => Error.Conflict(
            code: "Category.DuplicateName",
            description: $"Category with name '{name}' already exists."
        );

    }
}
