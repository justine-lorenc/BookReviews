using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Entities
{
    public class Exception
    {
        public Guid Id { get; set; }
        public string Error { get; set; }
        public string StackTrace { get; set; }
        public DateTime DateAdded { get; set; }
        public string Message { get; set; }
        public string Method { get; set; }
        public string Arguments { get; set; }
        public int UserId { get; set; }
    }
}
