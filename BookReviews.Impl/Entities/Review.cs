using BookReviews.Impl.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public float Rating { get; set; }
        public BookFormat BookFormat { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
        public int BookId { get; set; }
    }
}
