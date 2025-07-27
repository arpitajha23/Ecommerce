using Dapper;
using DataAccessLayer.DTO_s;
using InfrastructureLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
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
    public class ProfileRepository : IProfileRepository
    {
        private readonly IConfiguration _configuration;

        public ProfileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<UserProfileDto> GetUserDetailsbyId(long UserId)
        {

            try
            {
                const string sql = "SELECT * FROM fn_get_user_profile_details(@UserId)";

                var connectionString = _configuration.GetConnectionString("EcommerceDb");

                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                var result = await connection.QueryFirstOrDefaultAsync<UserProfileDto>(sql, new { UserId = UserId });
                return result;
            }
            catch (Exception ex)
            {
                // You can log the exception here using a logging framework like Serilog or NLog
                Console.WriteLine($"Error in GetUserDetailsbyId: {ex.Message}");
                throw new ApplicationException("An error occurred while fetching user details.", ex);
            }

        }

        public async Task<List<UserAddressDto>> GetUserAddressesAsync(int userId)
        {
            const string sql = "SELECT * FROM get_user_addresses(@UserId)";
            var connectionString = _configuration.GetConnectionString("EcommerceDb");

            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var result = await connection.QueryAsync<UserAddressDto>(sql, new { UserId = userId });
            return result.ToList();
        }
    }
}
