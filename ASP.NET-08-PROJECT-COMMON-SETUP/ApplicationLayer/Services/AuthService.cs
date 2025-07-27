using ApplicationLayer.Interfaces;
using DataAccessLayer.Common;
using DataAccessLayer.DTO_s;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.Services;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IOtpService _userOtpService;



        public AuthService(IConfiguration configuration, IUserRepository userRepository, EncryptionHelper encryptionHelper,
            EmailHelper emailHelper, ITokenService tokenService, IOtpService userOtpRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _encryptionHelper = encryptionHelper;
            _emailHelper = emailHelper;
            _tokenService = tokenService;
            _userOtpService = userOtpRepository;



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
                var repoResponse = _userRepository.UserLogin(userLoginDTO.Email, userLoginDTO.Password);


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

        public async Task<ServiceResponse> ForgotPassword(string email)
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

            //string token = GeneratePasswordResetToken(user);
            // Generate Token and OTP
            string token = GeneratePasswordResetToken(user);

            // 2. Generate OTP via PostgreSQL function using Dapper
            var otpSTRING = await _userOtpService.GenerateOtpAsync(user.Id);
            string otp = otpSTRING.Otp;

            //string resetLink = $"http://localhost:4200/reset-password?token={token}";
            //string resetLink = $"http://localhost:4200/endroute/resetlink?token={token}";
            string resetLink = $"http://localhost:4200/endroute/resetlink?token={token}&otpId={otpSTRING.UserOtpId}&userId={user.Id}";


            // 📨 Email 1: OTP Email
            // 4. Send OTP Email
            string otpBody = EmailHelper.GetOtpBody(otp);

            bool otpEmailSent = _emailHelper.SendEmail(user.Email, "Your OTP for Password Reset", otpBody);

            // 5. Send Link Email
            string linkBody = EmailHelper.GetResetLinkBody(resetLink);

            bool linkEmailSent = _emailHelper.SendEmail(user.Email, "Reset Password Link", linkBody);

            if (!otpEmailSent || !linkEmailSent)
            {
                return new ServiceResponse
                {
                    StatusCode = 500,
                    Message = "Failed to send OTP or reset link email"
                };
            }
           
            return new ServiceResponse
            {
                StatusCode = 200,
                Message = "OTP and Reset link sent to your email",
                Data = new
                {
                    OtpId = otpSTRING.UserOtpId, // Add this in your `GenerateOtpAsync` return
                    Token = token,
                    userId = user.Id,
                    Email = (user.Email)//EncryptionHelper.Decrypt(user.Email) // optional
                }
            };
        }

        public async Task<ServiceResponse> ResetPassword(string token, int otp, string newPassword, long otpId)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                if (jwtToken == null || !jwtToken.Claims.Any())
                    return new ServiceResponse { StatusCode = 400, Message = "Invalid token" };

                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (emailClaim == null)
                    return new ServiceResponse { StatusCode = 400, Message = "Email claim missing" };

                string email = emailClaim.Value;
                var user = _userRepository.GetUserByEmail(email);
                if (user == null)
                    return new ServiceResponse { StatusCode = 404, Message = "User not found" };

                bool isOtpValid = await _userOtpService.VerifyOtpAsync(user.Id, otpId, otp);

                if (!isOtpValid)
                    return new ServiceResponse { StatusCode = 400, Message = "Invalid or expired OTP" };

                string salt = GenerateHashFromPassword.GenerateSalt();
                string hashedPassword = GenerateHashFromPassword.GetHash(newPassword, salt);

                user.Password = hashedPassword;
                user.Salt = salt;

                //user.Passwordmodifiedat = DateTime.UtcNow;
                user.Passwordmodifiedat = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);


                // ✅ Update user in DB
                _userRepository.UpdateUserAsync(user);
                //_userRepository.UpdateUserAsync(user);

                return new ServiceResponse
                {
                    StatusCode = 200,
                    Message = "Password has been reset successfully."
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    StatusCode = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }


        public async Task<ServiceResponse> ValidateResetTokenAsync(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                if (jwtToken == null || !jwtToken.Claims.Any(c => c.Type == "reset"))
                {
                    return new ServiceResponse
                    {
                        StatusCode = 400,
                        Message = "Invalid token"
                    };
                }

                // Check expiry
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    return new ServiceResponse
                    {
                        StatusCode = 401,
                        Message = "Token expired"
                    };
                }

                return new ServiceResponse
                {
                    StatusCode = 200,
                    Message = "Valid token"
                };
            }
            catch (Exception)
            {
                return new ServiceResponse
                {
                    StatusCode = 401,
                    Message = "Invalid or expired token"
                };
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
                 expires: DateTime.UtcNow.AddMinutes(30),
        signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public async Task<ServiceResponse> ResendOtp(string email)
        //{
        //    var user = _userRepository.GetUserByEmail(email);

        //    if (user == null)
        //    {
        //        return new ServiceResponse { StatusCode = 404, Message = "User not found" };
        //    }

        //    string otp = await _userRepository.GenerateOtpAsync(user.Id);

        //    // Send Email
        //    string otpBody = $@"
        //<p>Hello,</p>
        //<p>Your new One-Time Password (OTP) is: <strong style='color:#10b981;font-size:18px'>{otp}</strong></p>
        //<p>This OTP will expire in 30 minutes.</p>";

        //    bool emailSent = _emailHelper.SendEmail(user.Email, "Resend OTP for Password Reset", otpBody);

        //    if (!emailSent)
        //    {
        //        return new ServiceResponse { StatusCode = 500, Message = "Failed to resend OTP" };
        //    }

        //    return new ServiceResponse { StatusCode = 200, Message = "OTP resent to your email." };
        //}

    }
}
