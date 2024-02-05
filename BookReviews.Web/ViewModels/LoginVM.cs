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
        [StringLength(100, ErrorMessage = "100 characters max")]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "50 characters max")]
        public string Password { get; set; }
    }
}