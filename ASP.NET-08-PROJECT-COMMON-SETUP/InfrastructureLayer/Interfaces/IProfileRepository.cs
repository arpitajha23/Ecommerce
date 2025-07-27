using DataAccessLayer.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces
{
    public interface IProfileRepository
    {
        Task<UserProfileDto> GetUserDetailsbyId(long UserId);

        Task<List<UserAddressDto>> GetUserAddressesAsync(int userId);

    }
}
