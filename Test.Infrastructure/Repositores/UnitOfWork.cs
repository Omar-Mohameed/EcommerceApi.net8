using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Entities;
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
        public readonly UserManager<AppUser> _userManager;
        public readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IGenerateTokenService _generateTokenService;





        // Instance of repositories
        public ICategoryRepo Categories { get; }
        public IProductRepo Products { get; }
        public ICustomerBasketRepo Baskets { get; }
        public IAuth Auth {get; }

        public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService, IMapper mapper, IConnectionMultiplexer redis,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IConfiguration configuration, IGenerateTokenService generateTokenService)
        {
            _context = context;
            _imageManagementService = imageManagementService;
            _mapper = mapper;
            _redis = redis;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
            _generateTokenService = generateTokenService;

            Categories = new CategoryRepo(_context);
            Products = new ProductRepo(_context, _mapper, _imageManagementService);
            Baskets = new CustomerBasketRepo(_redis);
            Auth = new AuthRepo(_userManager, _signInManager, _emailService, _configuration,_generateTokenService,_context);
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
