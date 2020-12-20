using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Utilities
{
    static public class UserHelper
    {
       public static bool IsValidData(UserPassViewModel loginModel)
        {
            if(string.IsNullOrEmpty(loginModel.Password) || loginModel.Password.Length<8)
                throw new Exception("Password is not valid");

            if (!RegexUtilities.IsValidEmail(loginModel.Email))
                throw new Exception("Email is not valid");
            return true;
        }
    }
}
