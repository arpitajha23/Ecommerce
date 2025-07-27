using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO_s
{
    public class GenerateOtpResultDto
    {
       public long UserOtpId { get; set; }
       public string Otp { get; set; }
       public long UserId { get; set; }

    }
}
