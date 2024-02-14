using BookReviews.Impl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookReviews.Web.Models
{
    public class ReviewInfo
    {
        public long BookId { get; set; }
        public int TotalReviews { get; set; }
        public decimal AverageRating { get; set; }
        public Review UserReview { get; set; }
        public List<Review> CommunityReviews { get; set; }
    }
}