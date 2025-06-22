using DataAccessLayer.DTO_s;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces
{
    public interface IUserRepository
    {
        ServiceResponse UserRegister(EndUserRegistration userRegistrationDTO);
        //ServiceResponse UserLogin(LoginDto userLoginDTO);
        ServiceResponse UserLogin(string email, string encryptedPassword);

        Appuser GetUserByEmail(string email);

        ServiceResponse UpdateUserAsync(Appuser user);


    }
}
