
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.ViewModel
{
    public class UserViewModel: BaseDomain
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }      
       

        public ICollection<string> Roles { get; set; }       
        
    }
}
