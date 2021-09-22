using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.DTOs
{
    public class RegistrationDto
    {
        [Required , MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required, MaxLength(50)]
        public string UserName { get; set; }
        [Required, MaxLength(156)]
        public string Email { get; set; }
        [Required, MaxLength(256)]
        public string Password { get; set; }


    }
}
