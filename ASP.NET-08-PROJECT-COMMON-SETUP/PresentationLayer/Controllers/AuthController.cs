using ApplicationLayer.Interfaces;
using DataAccessLayer.DTO_s;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PresentationLayer.Data;
using PresentationLayer.Models;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly EcommerceDbContext _context;

        public AuthController(IConfiguration configuration, IAuthService authService, EcommerceDbContext ecommerceDbContext)
        {
            _configuration = configuration;
            _authService = authService;
            _context = ecommerceDbContext;
        }

        [HttpPost("Registration")]
        [AllowAnonymous]
        public IActionResult Registration(EndUserRegistration userRegistrationDTO)
        {
            var result = _authService.UserRegister(userRegistrationDTO);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Login")]
        [AllowAnonymous]

        public IActionResult Login(LoginDto userLoginDTO)
        {
            var result = _authService.UserLogin(userLoginDTO);
            return StatusCode(result.StatusCode, result);
        }


        //Google Authentication Login
        [HttpGet("GoogleLogin")]
        [AllowAnonymous]

        public async Task< IActionResult> GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("GoogleResponse")]
        [AllowAnonymous]

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claimsIdentity = result.Principal?.Identities?.FirstOrDefault();
            if (claimsIdentity == null)
            {
                return BadRequest(new { success = false, message = "No identity found" });
            }

            var email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
            var name = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var googleId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (email == null)
            {
                return BadRequest(new { success = false, message = "Google login failed: Email not found." });
            }

            // Check if user exists
            var user = await _context.Appusers.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new Appuser
                {
                    Fullname = name,
                    Email = email,
                    Password = null,
                    Phone = null,
                    Isemailconfirmed = true,
                    Isactive = true,
                    Googleid = googleId,
                    Createdat = DateTime.Now,
                    Passwordmodifiedat = null
                };

                _context.Appusers.Add(user);
                await _context.SaveChangesAsync();
            }

            // 🔐 Generate JWT Token
            //var token = _tokenService.GenerateToken(user); // Create this method to generate JWT

            return Ok(new
            {
                success = true,
                message = "Google login successful",
                //token = token,
                email = user.Email,
                name = user.Fullname
            });
        }

        //google user logout
        [HttpGet("GoogleLogout")]
        [AllowAnonymous]

        public async Task<IActionResult> GoogleLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(GoogleDefaults.AuthenticationScheme);

            return Ok(new { message = "User logged out successfully" });
        }


        [HttpPost("forgot-password")]
        [AllowAnonymous]

        public IActionResult ForgotPassword([FromBody] string email)
        {
            var response = _authService.ForgotPassword(email);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        
        public IActionResult ResetPassword([FromBody] ResetPasswordDto model)
        {
            var response = _authService.ResetPassword(model.Token, model.NewPassword);
            return StatusCode(response.StatusCode, response);
        }
    }
}
