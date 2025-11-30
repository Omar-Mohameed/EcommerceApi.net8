using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Interfaces;
using Test.Core.Services;
using Test.Infrastructure.Data;
using Test.Infrastructure.Repositores;
using Test.Infrastructure.Repositores.Services;

namespace Test.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection Services, IConfiguration Configuration)
        {
            // Add services to the container.
            Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            Services.AddScoped(typeof(IGenericRepo‎<>), typeof(GenericRepo<>));

            // Register Unitofwork service
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddSingleton<IImageManagementService, ImageManagementService>();
            Services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            // apply redis cache
            Services.AddSingleton<IConnectionMultiplexer>(i =>
            {
                var config = ConfigurationOptions.Parse(Configuration.GetConnectionString("redis"));
                return ConnectionMultiplexer.Connect(config);
            });


            return Services;
        }
    }
}
