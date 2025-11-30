using AutoMapper;
using StackExchange.Redis;
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
        private readonly IConnectionMultiplexer _redis;


        // Instance of repositories
        public ICategoryRepo Categories { get; }
        public IProductRepo Products { get; }
        public ICustomerBasketRepo Baskets { get; }


        public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService, IMapper mapper, IConnectionMultiplexer redis)
        {
            _context = context;
            _imageManagementService = imageManagementService;
            _mapper = mapper;
            _redis = redis;

            Categories = new CategoryRepo(_context);
            Products = new ProductRepo(_context, _mapper, _imageManagementService);
            Baskets = new CustomerBasketRepo(_redis);
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
