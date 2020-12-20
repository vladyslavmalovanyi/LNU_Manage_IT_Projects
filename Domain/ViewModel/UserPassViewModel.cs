using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.ViewModel
{
   public class UserPassViewModel : UserViewModel
    {
        [Required]
        public string Password{get; set;}
    }
}
