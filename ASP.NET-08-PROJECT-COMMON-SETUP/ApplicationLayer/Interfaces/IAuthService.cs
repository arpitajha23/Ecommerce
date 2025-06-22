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
        ServiceResponse ForgotPassword(string email);

        ServiceResponse ResetPassword(string token, string newPassword);

    }
}
