
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModel
{
    public class UserViewModel: BaseDomain
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }      
       

        public ICollection<string> Roles { get; set; }       
        
    }
}
