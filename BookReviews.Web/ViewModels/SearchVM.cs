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
        public SearchCategory SearchCategory { get; set; }

        public string SearchTerm { get; set; }

        public List<Book> Results { get; set; }
    }
}