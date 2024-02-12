﻿using BookReviews.Impl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookReviews.Web.Models
{
    public class BookInfo
    {
        public Book Book { get; set; }
        public Review UserReview { get; set; }
        public List<Review> CommunityReviews { get; set; }
    }
}