
using Microsoft.EntityFrameworkCore;
using Test.Api.Middlewares;
using Test.Core.Interfaces;
using Test.Infrastructure;
using Test.Infrastructure.Data;
using Test.Infrastructure.Repositores;

namespace Test.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.infrastructureConfiguration(builder.Configuration);
            builder.Services.AddMemoryCache();
            // add cors policy
            builder.Services.AddCors(op =>
            {
                op.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200");
                });
            });

            // Register IMapper service with correct configuration
            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("CorsPolicy");

            app.UseMiddleware<ExceptionsMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
