using DataAccessLayer.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Interfaces
{
    public interface IAuthService
    {
        ServiceResponse UserRegister(EndUserRegistration userRegistrationDTO);
        ServiceResponse UserLogin(LoginDto userLoginDTO);
        Task<ServiceResponse> ForgotPassword(string email);

        Task<ServiceResponse> ResetPassword(string token, int otp, string newPassword, long otpId);
        Task<ServiceResponse> ValidateResetTokenAsync(string token);

    }
}
