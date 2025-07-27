using ApplicationLayer.Interfaces;
using ApplicationLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileservice;
        public ProfileController( IProfileService profileService)
        {
            _profileservice = profileService;

        }

        [HttpGet("GetUserDetailsById/{userId}")]
        public async Task<IActionResult> GetUserDetailsById(long userId) 
        {
            try
            {
                var result = await _profileservice.GetUserDetailsbyId(userId);

                if (result == null)
                    return NotFound(new { message = "User not found." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching user details.", error = ex.Message });
            }
        }

        [HttpGet("user-addresses/{userId}")]
        public async Task<IActionResult> GetUserAddresses(int userId)
        {
            var result = await _profileservice.GetAddressesByUserIdAsync(userId);
            return Ok(result);
        }

    }
}
