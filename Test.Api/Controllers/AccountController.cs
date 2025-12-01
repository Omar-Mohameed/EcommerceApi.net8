using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Test.Api.Helper;
using Test.Core.DTOS.AuthDTOS;
using Test.Core.Interfaces;

namespace Test.Api.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result =await work.Auth.RegisterAsync(registerDto);
            if(result != "done")
                return Ok(new ResponseApi(400, result));

            return Ok(new ResponseApi(200, "Registration successful! Please check your email to confirm your account."));
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await work.Auth.LoginAsync(loginDto);
            if (result != null && !result.Contains("Invalid") && !result.Contains("Please"))
            {
                // Login Success
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    IsEssential = true,
                    Domain = "localhost",
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Append("token",result,cookieOptions);
                return Ok(new ResponseApi(200, "Login Success",result));
            }
            return BadRequest(new ResponseApi(400, result));
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok(new ResponseApi(200, "Logged out successfully"));
        }

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            var accountdto = new ActiveAccountDTO { Email = email, Token = code };
            var result = await work.Auth.ActiveAccount(accountdto);

            if (result!= false)
                return Ok(new ResponseApi(200, "Email Confirmed!"));

            return BadRequest(new ResponseApi(400, "Invalid Token"));
        }
        [Authorize]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getusername")]
        public IActionResult GetUserName()
        {
            return Ok(new ResponseApi(200, User.Identity.Name));
        }
        [HttpGet("IsUserAuth")]
        public async Task<IActionResult> IsUserAuth()
        {

            return User.Identity.IsAuthenticated ? Ok(new ResponseApi(200,"Authorized User")) : BadRequest();
        }

    }
}
