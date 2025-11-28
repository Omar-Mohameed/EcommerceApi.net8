using ErrorOr;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Entities.Product;
using Test.Core.Interfaces;
using Test.Core.Services.Errors;
using Test.Infrastructure.Data;

namespace Test.Infrastructure.Repositores
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        private readonly AppDbContext context;

        public CategoryRepo(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<ErrorOr<Category>> AddAsync(Category category)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(category.Name) ||
                category.Name.Length < 3 ||
                category.Name.Length > 50)
            {
                return CategoryErrors.InvalidName;
            }
            // check if DuplicateName category
            if (await DuplicateName(category.Name))
            {
                return CategoryErrors.DuplicateName(category.Name);
            }
            
            await context.AddAsync(category);
            return category; // Success case
        }

        public async Task<bool> DuplicateName(string name)
        {
            return await context.Categories
            .AsNoTracking()
            .AnyAsync(c => c.Name.ToLower() == name.Trim().ToLower());
        }
    }
}
