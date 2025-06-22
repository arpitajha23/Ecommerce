using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO_s
{
    public class ForgetPasswordDtos
    {

        [Required, EmailAddress]
        public string Email { get; set; }

        public bool EmailSent { get; set;} 
    }
}
