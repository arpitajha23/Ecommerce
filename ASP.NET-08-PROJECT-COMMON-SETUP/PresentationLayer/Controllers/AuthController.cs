using ApplicationLayer.Interfaces;
using DataAccessLayer.DTO_s;
using InfrastructureLayer.Interfaces;
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
        private readonly ITokenService _tokenService;


        public AuthController(IConfiguration configuration, IAuthService authService, EcommerceDbContext ecommerceDbContext, ITokenService tokenService)
        {
            _configuration = configuration;
            _authService = authService;
            _context = ecommerceDbContext;
            _tokenService = tokenService;
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
        public async Task<IActionResult> Login(LoginDto userLoginDTO)
        {
            try
            {
                // 1. Proceed with your login logic
                var user = _authService.UserLogin(userLoginDTO);

                if (user == null)
                    return Unauthorized("Invalid credentials");

                return Ok(user); // or token response
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        //Google Authentication Login
        [HttpGet("GoogleLogin")]
        [AllowAnonymous]
        public async Task< IActionResult> GoogleLogin()
        {
            ////var redirectUrl = Url.Action("GoogleResponse");
            //var redirectUrl = "http://localhost:5052/api/Auth/GoogleResponse"; // Url.Action("GoogleResponse", "Auth", null, Request.Scheme);
            //var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            //return Challenge(properties, GoogleDefaults.AuthenticationScheme);

            var redirectUrl = Url.Action("GoogleResponse", "Auth", null, Request.Scheme); // safer
            //var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("GoogleResponse")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);


                if (!result.Succeeded || result.Principal == null)
                {
                    return BadRequest(new { success = false, message = "Google authentication failed." });
                }

                var claimsPrincipal = result.Principal;
                var claimsIdentity = claimsPrincipal.Identities?.FirstOrDefault();
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

                var token = _tokenService.GenerateToken(user);

                // Cookie-based sign-in (optional)
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true
                    });

                // ✅ Redirect to Angular app with JWT token
                var redirectUrl = $"http://localhost:4200/endroute/dashboard?token={token}&email={email}&name={Uri.EscapeDataString(name)}";
                return Redirect(redirectUrl);
                //var redirectUrl = $"http://localhost:4200/endroute/dashboard?token={token}";
                //return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Exception occurred: " + ex.Message });
            }
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


        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailRequest request)
        {
            var response = await _authService.ForgotPassword(request.Email);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var response = await _authService.ResetPassword(model.Token, model.Otp, model.NewPassword, model.otpId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken([FromQuery] string token)
        {
            var result = await _authService.ValidateResetTokenAsync(token);
            return StatusCode(result.StatusCode, result);
        }

    }
}
