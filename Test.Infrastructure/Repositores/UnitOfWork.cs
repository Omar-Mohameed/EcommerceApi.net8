using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Interfaces;
using Test.Infrastructure.Data;

namespace Test.Infrastructure.Repositores
{
    public class UnitOfWork : IUnitOfWork
    {
        // instance of serveces
        private readonly AppDbContext _context;

        // Instance of repositories
        public ICategoryRepo Categories { get; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Categories = new CategoryRepo(_context);
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        // تنظيف الموارد
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
