using DataAccessLayer.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Interfaces
{
    public interface IOtpService
    {
        Task<GenerateOtpResultDto> GenerateOtpAsync(long userId);

        Task<bool> VerifyOtpAsync(long userId, long userOtpId, int otp);
        Task<ServiceResponse> ResendOtpAsync(long userId);

    }
}
