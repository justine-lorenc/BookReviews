using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Models
{
    public class Genre
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public bool IsFiction { get; set; }
    }
}
