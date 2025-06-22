using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO_s
{
    public class EndUserRegistration
    {
        public string Fullname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; } 

        public string? Phone { get; set; }

        public string? Googleid { get; set; }

        public bool? Isactive { get; set; }
        public bool? Isemailconfirmed { get; set; }

        public DateTime? Createdat { get; set; }

        public DateTime? Passwordmodifiedat { get; set; }
    }
}
