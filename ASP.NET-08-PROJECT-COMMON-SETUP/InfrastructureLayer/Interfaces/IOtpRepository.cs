using DataAccessLayer.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces
{
    public interface IOtpRepository
    {
        Task<GenerateOtpResultDto> GenerateOtpAsync(long userId);
        Task<bool> VerifyOtpAsync(long userId, long userOtpId, int otp);
        Task<ResendOtpResultDto> ResendOtpAsync(long userId);


    }
}
