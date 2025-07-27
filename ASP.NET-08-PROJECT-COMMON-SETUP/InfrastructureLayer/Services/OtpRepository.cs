using Dapper;
using DataAccessLayer.DTO_s;
using InfrastructureLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Services
{
    public class OtpRepository : IOtpRepository
    {
        private readonly IDbConnection _dbConnection;

        public OtpRepository(IConfiguration configuration)
        {
            _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("EcommerceDb"));
        }

        public async Task<GenerateOtpResultDto> GenerateOtpAsync(long userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_user_id", userId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<GenerateOtpResultDto>(
                     "SELECT * FROM generate_otp(@p_user_id)",
                    new { p_user_id = userId },
                    commandType: CommandType.Text
                );

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> VerifyOtpAsync(long userId, long userOtpId, int otp)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_userid", userId);
            parameters.Add("p_userotpid", userOtpId);
            parameters.Add("p_otp", otp);

            var result = await _dbConnection.ExecuteScalarAsync<bool>(
                "SELECT verify_loginotp(@p_userid, @p_userotpid, @p_otp);",
                parameters,
                commandType: CommandType.Text
            );

            return result;

        }

        public async Task<ResendOtpResultDto> ResendOtpAsync(long userId)
            {
            var parameters = new DynamicParameters();
            parameters.Add("p_userid", userId);

            return await _dbConnection.QueryFirstOrDefaultAsync<ResendOtpResultDto>(
                "SELECT * FROM resend_loginotp(@p_userid);",
                parameters,
                commandType: CommandType.Text
            );
        }

    }
}
