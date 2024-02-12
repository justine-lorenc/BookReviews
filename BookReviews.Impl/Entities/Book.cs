using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Entities
{
    public class Book
    {
        public Book()
        {
            Authors = new List<Author>();
        }

        public long Id { get; set; }  // ISBN-13, hyphens removed
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public short Pages { get; set; }
        public DateTime DatePublished { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public List<Author> Authors { get; set; }
    }
}
