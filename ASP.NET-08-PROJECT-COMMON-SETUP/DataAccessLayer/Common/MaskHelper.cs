using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common
{
    public class MaskHelper
    {
        private string MaskEmail(string email)
        {
            var parts = email.Split('@');
            if (parts[0].Length <= 2) return "***@" + parts[1];
            return parts[0].Substring(0, 2) + "****@" + parts[1];
        }
    }
}
