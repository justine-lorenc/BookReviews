using BookReviews.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookReviews.Web.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Required")]
        [StringLength(Globals.MaxLengths.User.EmailAddress, ErrorMessage = "100 characters max")]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Required")]
        [StringLength(Globals.MaxLengths.User.Password, ErrorMessage = "50 characters max")]
        public string Password { get; set; }
    }
}