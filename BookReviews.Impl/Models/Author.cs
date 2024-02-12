using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Models
{
    public class Author
    {
        public int Id { get; set; }

        // Note: This is not normalized because book APIs typically return this as a single field.
        // Names can follow a variety of formats, so it can be difficult to parse which parts are
        // a first, middle, or last name.
        // Ex: Gabriel Garcia Marquez, Miguel de Cervantes, L.M. Montgomery, F. Scott Fitzgerald, Edgar Allen Poe
        public string Name { get; set; }
    }
}
