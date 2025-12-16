using ECommerce.Core.DTOS.AuthDTOS;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Core.Shared;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Repositores
{
    public class AuthRepo : IAuth
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;


        private readonly IEmailService _emailService;

        private readonly IConfiguration _configuration;
        private readonly IGenerateTokenService generateTokenService;
        private readonly AppDbContext _context;


        public AuthRepo(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IConfiguration configuration, IGenerateTokenService generateTokenService, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
            this.generateTokenService = generateTokenService;
            _context = context;
        }


        public async Task<string> RegisterAsync(RegisterDto registorDto)
        {
            if (registorDto == null) return null;
            // check username istoken
            if (await _userManager.FindByNameAsync(registorDto.UserName) != null)
                return "Username is already taken";
            // check email is token
            if (await _userManager.FindByEmailAsync(registorDto.Email) != null)
                return "Email is already registered";

            var user = new AppUser
            {
                UserName = registorDto.UserName,
                Email = registorDto.Email,
                DisplayName = registorDto.DisplayName
            };
            var result = await _userManager.CreateAsync(user, registorDto.Password); // // store in db with hashed password but (not allowed to login if email is not confirmed)
            if (!result.Succeeded)
                return result.Errors.ToList()[0].Description;

            //  Send confirmation email 
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // Send email
            await SendEmail(user.Email, token, "confirmemail",
                "Confirm your email",
                "Please confirm your email by clicking the button below.");

            return "done";
        }
        public async Task SendEmail(string email, string code, string component, string subject, string message)
        {

            var emailDTO = new EmailDto(
                email,
                _configuration["EmailSetting:From"],
                subject,
         EmailStringBody.send(email, code, component, message)
            );
            await _emailService.SendEmailAsync(emailDTO);
        }

        public async Task<string> LoginAsync(LoginDto loginDTO)
        {
            if (loginDTO == null) return null;
            var FindUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (FindUser == null)
                return "Invalid Email or Password";
            // Check if email is confirmed(not allowed to login if email is not confirmed)
            if (!FindUser.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(FindUser);
                await SendEmail(FindUser.Email, token, "confirmemail",
                "Confirm your email",
                "Please confirm your email by clicking the button below.");

                return "Please confirm your email first!, check your email.";
            }

            // check password
            var result = await _signInManager.CheckPasswordSignInAsync(FindUser,    // CheckPasswordSignInAsync method to only verify the user's password(without signing them in,without create token).
                loginDTO.Password, false);
            if (result.Succeeded)
            {
                // Generate JWT Token
                var token = generateTokenService.GetAndCreateToken(FindUser);
                return token;
            }
            return "Invalid Email or Password";
        }

        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            var user = await _userManager.FindByEmailAsync(accountDTO.Email);

            if (user == null)
                return false;

            // Decode token
            var decodedToken = Uri.UnescapeDataString(accountDTO.Token);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return true;

            return false;
        }
        public async Task<bool> ResendConfirmation(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            if (user.EmailConfirmed) return true;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "confirmemail", "ActiveEmail", "Click to activate your email");

            return true;
        }
        // forget password
        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await SendEmail(user.Email, token, "resetpassword",
                "Reset your password",
                "You can reset your password by clicking the button below.");
            return true;
        }
        // reset password
        public async Task<string> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var findUser = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (findUser == null)
                return null;
            // Decode token
            var decodedToken = Uri.UnescapeDataString(resetPasswordDto.Token);
            var result = await _userManager.ResetPasswordAsync(findUser, decodedToken, resetPasswordDto.Password);
            if (result.Succeeded)
                return "done";
            return result.Errors.ToList()[0].Description;
        }

        // Address
        public async Task<Address> getUserAddress(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.AppUserId == user.Id);
            return address;
        }

        public async Task<bool> UpdateAddress(string email, Address address)
        {
            // Get user
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            // Get existing address
            var existingAddress = await _context.Addresses
                                                .FirstOrDefaultAsync(a => a.AppUserId == user.Id);

            if (existingAddress is null)
            {
                // Create new address
                address.AppUserId = user.Id;
                await _context.Addresses.AddAsync(address);
            }
            else
            {
                _context.Entry(existingAddress).State = EntityState.Detached; // Stop Tracking this entity(existingaddress) 
                address.Id = existingAddress.Id;
                address.AppUserId = existingAddress.AppUserId;
                _context.Addresses.Update(address);
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
