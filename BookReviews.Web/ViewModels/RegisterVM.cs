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
        [StringLength(100, ErrorMessage = "100 characters max")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "50 characters max")]
        [RegularExpression(@"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%^&*-_=+]).{10,}", 
            ErrorMessage = "Please enter a valid password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "50 characters max")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "50 characters max")]
        [RegularExpression(@"[a-zA-Z-'\s]+", ErrorMessage = "Please enter a valid name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "50 characters max")]
        [RegularExpression(@"[a-zA-Z-'\s]+", ErrorMessage = "Please enter a valid name")]
        public string LastName { get; set; }
    }
}