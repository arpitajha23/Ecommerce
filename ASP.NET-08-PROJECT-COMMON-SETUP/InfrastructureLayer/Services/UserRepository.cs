using DataAccessLayer.Common;
using DataAccessLayer.DTO_s;
using InfrastructureLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using PresentationLayer.Data;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly EcommerceDbContext _context;
        private readonly EncryptionHelper _encryptionHelper;

        public UserRepository(IConfiguration configuration, EcommerceDbContext context, EncryptionHelper encryptionHelper)
        {
            _configuration = configuration;
            _context = context;
            _encryptionHelper = encryptionHelper;
        }

        public ServiceResponse UserRegister(EndUserRegistration userRegistrationDTO)
        {
            try
            {
                if (userRegistrationDTO != null)
                {
                    var users = new Appuser
                    {
                        Fullname = userRegistrationDTO.Fullname,
                        Email = userRegistrationDTO.Email,
                        Password = _encryptionHelper.Encrypt(userRegistrationDTO.Password),
                        Phone = userRegistrationDTO.Phone,
                        Isemailconfirmed = false,
                        Isactive = true,
                        Googleid = userRegistrationDTO.Googleid,
                        Createdat = DateTime.Now,
                        Passwordmodifiedat = DateTime.Now
                    };

                    _context.Appusers.Add(users);
                    _context.SaveChanges();

                    // Send Welcome Email
                    var emailHelper = new EmailHelper(_configuration);
                    var subject = "Welcome to QuickCart!";
                    var body = EmailHelper.GetWelcomeBody(users.Fullname);
                    emailHelper.SendEmail(users.Email, subject, body);


                    return new ServiceResponse
                    {
                        StatusCode = 200,
                        Data = null,
                        Message = "User registered successfully"
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        StatusCode = 400,
                        Data = null,
                        Message = "Invalid registration data"
                    };
                }
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

        //public ServiceResponse UserLogin(LoginDto userLoginDTO)
        public ServiceResponse UserLogin(string email, string encryptedPassword)
        {
            try
            {
                var user = _context.Appusers.FirstOrDefault(u => u.Email == email && u.Password == encryptedPassword);
                if (user != null)
                {
                    return new ServiceResponse
                    {
                        StatusCode = 200,
                        Data = user,
                        Message = "User found"
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        StatusCode = 401,
                        Data = null,
                        Message = "Invalid email or password"
                    };
                }
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

        public Appuser GetUserByEmail(string email)
        {
            return _context.Appusers.FirstOrDefault(u => u.Email == email);
        }

        public ServiceResponse UpdateUserAsync(Appuser user)
        {
            _context.Appusers.Update(user);
            _context.SaveChanges();

            return new ServiceResponse
            {
                StatusCode = 200,
                Message = "User updated successfully",
                Data = user
            };
        }

    }
}
