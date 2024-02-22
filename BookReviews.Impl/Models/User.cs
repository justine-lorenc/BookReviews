using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookReviews.Impl.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public bool IsValid(out string errorMessage)
        {
            errorMessage = String.Empty;

            if (String.IsNullOrWhiteSpace(EmailAddress))
            {
                errorMessage = "Email address is null or empty";
                return false;
            }
            else if (EmailAddress.Length > Globals.MaxLengths.User.EmailAddress)
            {
                errorMessage = "Email address exceeds max length";
                return false;
            }
            else if (!Regex.IsMatch(EmailAddress, Globals.RegularExpressions.EmailAddress))
            {
                errorMessage = $"Email address has invalid format: {EmailAddress}";
                return false;
            }
            else if (String.IsNullOrWhiteSpace(FirstName))
            {
                errorMessage = "First name is null or empty";
                return false;
            }
            else if (FirstName.Length > Globals.MaxLengths.User.FirstName)
            {
                errorMessage = "First name exceeds max length";
                return false;
            }
            else if (!Regex.IsMatch(FirstName, Globals.RegularExpressions.FirstName))
            {
                errorMessage = $"First name has invalid format: {FirstName}";
                return false;
            }
            else if (String.IsNullOrWhiteSpace(LastName))
            {
                errorMessage = "Last name is null or empty";
                return false;
            }
            else if (LastName.Length > Globals.MaxLengths.User.LastName)
            {
                errorMessage = "Last name exceeds max length";
                return false;
            }
            else if (!Regex.IsMatch(LastName, Globals.RegularExpressions.LastName))
            {
                errorMessage = $"Last name has invalid format: {LastName}";
                return false;
            }

            return true;
        }
    }
}
