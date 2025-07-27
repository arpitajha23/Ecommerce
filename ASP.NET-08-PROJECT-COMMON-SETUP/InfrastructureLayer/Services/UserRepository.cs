using DataAccessLayer.Common;
using DataAccessLayer.DTO_s;
using InfrastructureLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PresentationLayer.Data;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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
                  
                   string saltstring = GenerateHashFromPassword.GenerateSalt();
                   string hashedPassword = GenerateHashFromPassword.GetHash(userRegistrationDTO.Password, saltstring);
                   var users = new Appuser
                        {
                            Fullname = userRegistrationDTO.Fullname,
                            Email = userRegistrationDTO.Email,
                            Password = hashedPassword,
                            Salt = saltstring,
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
                var user = _context.Appusers.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    // Get stored salt and hash the input password

                    string hashedInputPassword = GenerateHashFromPassword.GetHash(encryptedPassword, user.Salt);

                    if (user.Password == hashedInputPassword)
                    {
                        return new ServiceResponse
                        {
                            StatusCode = 200,
                            Data = user,
                            Message = "Login successful"
                        };
                    }
                    else
                    {
                        return new ServiceResponse
                        {
                            StatusCode = 401,
                            Data = null,
                            Message = "Invalid password"
                        };
                    }
                }
                else
                {
                    return new ServiceResponse
                    {
                        StatusCode = 404,
                        Data = null,
                        Message = "User not found"
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
        public async Task<Appuser> GetUserByIdAsync(long userId)
        {
            return await _context.Appusers.FirstOrDefaultAsync(u => u.Id == userId);
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

        //public async Task<string> GenerateOtpAsync(int userId, string purpose, int validityMinutes)
        //{
        //    using var connection = new NpgsqlConnection(_configuration.GetConnectionString("EcommerceDb"));
        //    await connection.OpenAsync();

        //    var result = await connection.ExecuteScalarAsync<string>(
        //        "SELECT generate_otp(@UserId, @Purpose, @Minutes)",
        //        new { UserId = userId, Purpose = purpose, Minutes = validityMinutes }
        //    );

        //    return result;
        //}

        //public async Task<string?> GetLatestValidOtpAsync(int userId, string purpose)
        //{
        //    using var connection = new NpgsqlConnection(_configuration.GetConnectionString("EcommerceDb"));
        //    await connection.OpenAsync();

        //    var result = await connection.ExecuteScalarAsync<string>(
        //        "SELECT get_latest_valid_otp(@UserId, @Purpose)",
        //        new { UserId = userId, Purpose = purpose }
        //    );

        //    return result;
        //}

        //public async Task<bool> VerifyOtpAsync(int userId, string otp, string purpose)
        //{
        //    using var connection = new NpgsqlConnection(_configuration.GetConnectionString("EcommerceDb"));
        //    await connection.OpenAsync();

        //    var result = await connection.ExecuteScalarAsync<bool>(
        //        "SELECT verify_otp(@UserId, @Otp, @Purpose)",
        //        new { UserId = userId, Otp = otp, Purpose = purpose }
        //    );

        //    return result;
        //}

      
    }
}
