using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO_s
{
    public class ResendOtpResultDto
    {
        public long UserOtpId { get; set; }
        public int Otp { get; set; }
        public long UserId { get; set; }
    }

}
