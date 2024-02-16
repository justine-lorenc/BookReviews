using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookReviews.Web.Models
{
    public class GenreRead
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Percent { get; set; }
    }
}