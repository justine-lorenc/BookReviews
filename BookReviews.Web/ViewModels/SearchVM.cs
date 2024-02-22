using BookReviews.Impl;
using BookReviews.Impl.Models;
using BookReviews.Impl.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookReviews.Web.ViewModels
{
    public class SearchVM
    {
        [Required(ErrorMessage = "Required")]
        public SearchCategory SearchCategory { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(Globals.MaxLengths.SearchTerm, ErrorMessage = "100 characters max")]
        public string SearchTerm { get; set; }

        public List<Book> Results { get; set; }
    }
}