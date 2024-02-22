using BookReviews.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookReviews.Web.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Required")]
        [StringLength(Globals.MaxLengths.User.EmailAddress, ErrorMessage = "100 characters max")]
        [RegularExpression(Globals.RegularExpressions.EmailAddress, ErrorMessage = "Please enter a valid email")]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Required")]
        [StringLength(Globals.MaxLengths.User.Password, ErrorMessage = "50 characters max")]
        [RegularExpression(Globals.RegularExpressions.Password, ErrorMessage = "Please enter a valid password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Required")]
        [StringLength(Globals.MaxLengths.User.Password, ErrorMessage = "50 characters max")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Required")]
        [StringLength(Globals.MaxLengths.User.FirstName, ErrorMessage = "50 characters max")]
        [RegularExpression(Globals.RegularExpressions.FirstName, ErrorMessage = "Please enter a valid name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Required")]
        [StringLength(Globals.MaxLengths.User.LastName, ErrorMessage = "50 characters max")]
        [RegularExpression(Globals.RegularExpressions.LastName, ErrorMessage = "Please enter a valid name")]
        public string LastName { get; set; }
    }
}