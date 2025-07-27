using ApplicationLayer.Interfaces;
using DataAccessLayer.Common;
using DataAccessLayer.DTO_s;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _userOtpRepository;
        private readonly IUserRepository _userRepository;
        private readonly EmailHelper _emailHelper;


        public OtpService(IOtpRepository userOtpRepository, IUserRepository userRepository, EmailHelper emailHelper)
        {
            _userOtpRepository = userOtpRepository;
            _userRepository = userRepository;
            _emailHelper = emailHelper;
        }

        public async Task<GenerateOtpResultDto> GenerateOtpAsync(long userId)
        {
            return await _userOtpRepository.GenerateOtpAsync(userId);
        }

        public async Task<bool> VerifyOtpAsync(long userId, long userOtpId, int otp)
        {
          
           return await _userOtpRepository.VerifyOtpAsync(userId, userOtpId, otp);
        }
        public async Task<ServiceResponse> ResendOtpAsync(long userId)
        {
            //return await _userOtpRepository.ResendOtpAsync(userId);
            try
            {
                // Step 1: Call DB to get the new OTP (but do not expose it in response)
                var otpResult = await _userOtpRepository.ResendOtpAsync(userId);

                    if (otpResult == null)
                    {
                        return new ServiceResponse
                        {
                            StatusCode = 404,
                            Message = "Failed to resend OTP. Please try again.",
                            Data = null
                        };
                    }

                // Step 2: Get the user details to fetch email
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new ServiceResponse
                    {
                        StatusCode = 404,
                        Message = "User not found.",
                        Data = null
                    };
                }
                // Step 3: Compose OTP email
                string otpBody = EmailHelper.GetOtpBody(otpResult.Otp.ToString()); // otpResult.Otp is only used internally
                bool otpEmailSent = _emailHelper.SendEmail(user.Email, "Your OTP for Password Reset", otpBody);

                if (!otpEmailSent)
                {
                    return new ServiceResponse
                    {
                        StatusCode = 500,
                        Message = "OTP generated but failed to send email.",
                        Data = null
                    };
                }

                // Step 4: Return success without OTP in response
                return new ServiceResponse
                {
                    StatusCode = 200,
                    Message = "OTP has been resent successfully.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                // Handle/log error accordingly
                return new ServiceResponse
                {
                    StatusCode = 500,
                    Message = $"Error occurred: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
