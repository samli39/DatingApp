using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DTO
{
    public class UserForRegister
    {
        [Required]
        public string Username { get; set; }
        [StringLength(8,MinimumLength =3, ErrorMessage="password  is between  4 to 8 character")]
        public string Password { get; set; }
    }
}
