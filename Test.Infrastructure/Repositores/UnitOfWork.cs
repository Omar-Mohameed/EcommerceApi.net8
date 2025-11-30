using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Interfaces;
using Test.Core.Services;
using Test.Infrastructure.Data;
using Test.Infrastructure.Repositores.Services;

namespace Test.Infrastructure.Repositores
{
    public class UnitOfWork : IUnitOfWork
    {
        // instance of serveces
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;

        // Instance of repositories
        public ICategoryRepo Categories { get; }
        public IProductRepo Products { get; }
        public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService, IMapper mapper)
        {
            _context = context;
            _imageManagementService = imageManagementService;
            _mapper = mapper;

            Categories = new CategoryRepo(_context);
            Products = new ProductRepo(_context,_mapper, _imageManagementService);
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
