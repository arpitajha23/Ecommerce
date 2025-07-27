using ApplicationLayer.Interfaces;
using DataAccessLayer.DTO_s;
using InfrastructureLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        public async Task<UserProfileDto> GetUserDetailsbyId(long UserId)
        {
            var result = await _profileRepository.GetUserDetailsbyId(UserId);
            return result;

        }
        public async Task<List<UserAddressDto>> GetAddressesByUserIdAsync(int userId)
        {
            var result = await _profileRepository.GetUserAddressesAsync(userId);
            return result;
        }
    }
}
