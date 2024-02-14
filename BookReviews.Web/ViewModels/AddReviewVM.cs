using BookReviews.Impl.Models;
using BookReviews.Impl.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookReviews.Web.ViewModels
{
    public class AddReviewVM
    {
        public Book Book { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Star Rating")]
        [Required(ErrorMessage = "Required")]
        public float Rating { get; set; }

        [Display(Name = "Format Read")]
        [Required(ErrorMessage = "Required")]
        public BookFormat BookFormat { get; set; }

        public List<Genre> Genres { get; set; }

        [Display(Name = "Genre")]
        [Required(ErrorMessage = "Required")]
        public short GenreId { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(1000, ErrorMessage = "Max 1000 characters")]
        public string Notes { get; set; }
    }
}