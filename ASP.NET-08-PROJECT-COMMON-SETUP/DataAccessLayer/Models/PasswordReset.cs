using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ResetToken { get; set; }
        public string OTP { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; } = false;

        public Appuser User { get; set; }
    }
}
