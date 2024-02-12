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
        public string Notes { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
        public User Author { get; set; }
        public Genre Genre { get; set; }
        public BookFormat BookFormat { get; set; }
    }
}
