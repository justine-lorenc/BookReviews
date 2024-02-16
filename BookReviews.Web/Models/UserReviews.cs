using BookReviews.Impl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookReviews.Web.Models
{
    public class UserReviews
    {
        public UserReviews()
        {
            FormatsRead = new List<FormatRead>();
            GenresRead = new List<GenreRead>();
            Reviews = new List<Review>();
            Years = new Dictionary<int, string>();
        }

        public int UserId { get; set; }
        public int Year { get; set; }
        public Dictionary<int, string> Years { get; set; }
        public int TotalBooks { get; set; }
        public int TotalPages { get; set; }
        public decimal AverageRating { get; set; }
        public List<FormatRead> FormatsRead { get; set; }
        public int TotalFiction { get; set; }
        public decimal FictionPercent { get; set; }
        public int TotalNonfiction { get; set; }
        public decimal NonfictionPercent { get; set; }
        public List<GenreRead> GenresRead { get; set; }
        public List<Review> Reviews { get; set; }
    }
}