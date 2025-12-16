using ECommerce.Core.Entities;
using ECommerce.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositores.Services
{
    public class GenerateTokenService : IGenerateTokenService
    {
        private readonly IConfiguration configuration;

        public GenerateTokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GetAndCreateToken(AppUser user)
        {
            // Claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            // secret key
            string secret = configuration["Token:Secret"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentNullException("Token secret is not configured");
            }
            // signature algorithm
            byte[] key = Encoding.ASCII.GetBytes(secret);// encoding key
            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            // token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = credentials,
                Issuer = configuration["Token:Issuer"],
                NotBefore = DateTime.UtcNow // token valid from now not before
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
