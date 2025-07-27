using ApplicationLayer.Interfaces;
using DataAccessLayer.DTO_s;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _userOtpService;

        public OtpController(IOtpService userOtpService)
        {
            _userOtpService = userOtpService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateOtp([FromBody] long userId)
        {
            var result = await _userOtpService.GenerateOtpAsync(userId);
            return Ok(result);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var success = await _userOtpService.VerifyOtpAsync(dto.UserId, dto.UserOtpId, dto.Otp);
            return Ok(new { verified = success });
        }

        [HttpPost("resend")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpResultDto request)
        {
            var result = await _userOtpService.ResendOtpAsync(request.UserId);
            return Ok(result);
        }
    }
}
