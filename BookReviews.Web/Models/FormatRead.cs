using BookReviews.Impl.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookReviews.Web.Models
{
    public class FormatRead
    {
        public BookFormat Format { get; set; }
        public int Count { get; set; }
        public decimal Percent { get; set; }
    }
}