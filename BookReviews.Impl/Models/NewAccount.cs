using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookReviews.Impl.Models
{
    public class NewAccount : User
    {
        public string Password { get; set; }

        public new bool IsValid(out string errorMessage)
        {
            errorMessage = String.Empty;

            if (!base.IsValid(out errorMessage))
            {
                return false;
            }
            else if (String.IsNullOrWhiteSpace(Password))
            {
                errorMessage = "Password is null or empty";
                return false;
            }
            else if (Password.Length > Globals.MaxLengths.User.Password)
            {
                errorMessage = "Password exceeds max length";
                return false;
            }
            else if (!Regex.IsMatch(Password, Globals.RegularExpressions.Password))
            {
                errorMessage = "Password has invalid format";
                return false;
            }

            return true;
        }
    }
}
