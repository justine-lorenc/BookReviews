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
        [Display(Name = "Search By")]
        public SearchCategory SearchCategory { get; set; }

        [Display(Name = "Search Term")]
        public string SearchTerm { get; set; }

        public List<Book> Results { get; set; }
    }
}