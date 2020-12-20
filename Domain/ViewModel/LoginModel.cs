using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModel
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
