using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Entities;
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

            Services.AddScoped<IEmailService, EmailService>();
            //register token
            Services.AddScoped<IGenerateTokenService, GenerateTokenService>();

            // apply redis cache
            Services.AddSingleton<IConnectionMultiplexer>(i =>
            {
                var config = ConfigurationOptions.Parse(Configuration.GetConnectionString("redis"));
                return ConnectionMultiplexer.Connect(config);
            });

            Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
        .AddCookie(x =>
        {
            x.Cookie.Name = "token";
            x.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Secret"])),
                ValidateIssuer = true,
                ValidIssuer = Configuration["Token:Issuer"],
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            x.Events = new JwtBearerEvents
            {

                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["token"];
                    context.Token = token;
                    return Task.CompletedTask;
                }
            };
        });



            return Services;
        }
    }
}
