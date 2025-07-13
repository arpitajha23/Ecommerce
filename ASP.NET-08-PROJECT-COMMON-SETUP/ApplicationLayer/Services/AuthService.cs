using ApplicationLayer.Interfaces;
using DataAccessLayer.Common;
using DataAccessLayer.DTO_s;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly EncryptionHelper _encryptionHelper;
        private readonly EmailHelper _emailHelper;
        private readonly ITokenService _tokenService;



        public AuthService(IConfiguration configuration, IUserRepository userRepository, EncryptionHelper encryptionHelper, EmailHelper emailHelper, ITokenService tokenService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _encryptionHelper = encryptionHelper;
            _emailHelper = emailHelper;
            _tokenService = tokenService;


        }

        public ServiceResponse UserRegister(EndUserRegistration userRegistrationDTO)
        {
            return _userRepository.UserRegister(userRegistrationDTO);
        }

        public ServiceResponse UserLogin(LoginDto userLoginDTO)
        {
            if (userLoginDTO == null || string.IsNullOrEmpty(userLoginDTO.Email) || string.IsNullOrEmpty(userLoginDTO.Password))
            {
                return new ServiceResponse
                {
                    StatusCode = 400,
                    Data = null,
                    Message = "Invalid login data"
                };
            }
            try
            {
                string encryptedPassword = _encryptionHelper.Encrypt(userLoginDTO.Password);

                var repoResponse = _userRepository.UserLogin(userLoginDTO.Email, encryptedPassword);

                if (repoResponse.StatusCode != 200)
                    return repoResponse;

                var user = (Appuser)repoResponse.Data;

                string token = _tokenService.GenerateToken(user);

                return new ServiceResponse
                {
                    StatusCode = 200,
                    Data = new
                    {
                        User = user,
                        Token = token
                    },
                    Message = "User logged in successfully"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    StatusCode = 500,
                    Data = null,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public ServiceResponse ForgotPassword(string email)
        {
            var user = _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                return new ServiceResponse
                {
                    StatusCode = 404,
                    Message = "User not found"
                };
            }

            string token = GeneratePasswordResetToken(user);

            string resetLink = $"http://localhost:4200/reset-password?token={token}";

            string emailBody = EmailHelper.GetForgotPasswordBody(resetLink);

           
            var placeholders = new Dictionary<string, string>
            {
                { "ResetLink", resetLink }
            };

            bool emailSent = _emailHelper.SendEmail(user.Email, "Reset Your Password", emailBody);

            if (!emailSent)
            {
                return new ServiceResponse
                {
                    StatusCode = 500,
                    Message = "Failed to send reset email"
                };
            }
            return new ServiceResponse
            {
                StatusCode = 200,
                Message = "Reset password email sent"
            };
        }

        public ServiceResponse ResetPassword(string token, string newPassword)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                if (jwtToken == null || !jwtToken.Claims.Any())
                {
                    return new ServiceResponse { StatusCode = 400, Message = "Invalid token" };
                }

                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email");
                if (emailClaim == null)
                {
                    return new ServiceResponse { StatusCode = 400, Message = "Email claim missing" };
                }

                string email = emailClaim.Value;

                var user = _userRepository.GetUserByEmail(email);

                if (user == null)
                {
                    return new ServiceResponse
                    {
                        StatusCode = 404,
                        Message = "User not found"
                    };
                }

                user.Password = _encryptionHelper.Encrypt(newPassword);
                user.Passwordmodifiedat = DateTime.Now;

                _userRepository.UpdateUserAsync(user);

                return new ServiceResponse { StatusCode = 200, Message = "Password reset successful" };
            }
            catch (Exception ex)
            {
                return new ServiceResponse { StatusCode = 500, Message = $"Error: {ex.Message}" };
            }
        }
        // Generates JWT for password reset
        private string GeneratePasswordResetToken(Appuser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTConfigration:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("reset", "true")
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTConfigration:Issuer"],
                audience: _configuration["JWTConfigration:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
