using DataAccessLayer.DTO_s;
using InfrastructureLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InfrastructureLayer.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public TokenService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
           _configuration = configuration;
           _httpClientFactory = httpClientFactory;

        }

        public string GenerateToken(Appuser user)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Fullname ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? "")
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTConfigration:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                issuer: _configuration["JWTConfigration:Issuer"],
                audience: _configuration["JWTConfigration:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

    }
}