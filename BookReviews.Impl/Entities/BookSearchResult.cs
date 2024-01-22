using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Entities
{
    public class BookSearchResult
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public short PageCount { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Description { get; set; }
        public List<IndustryIdentifier> IndustryIdentifiers { get; set; }
        public ImageLinks ImageLinks { get; set; }
        public List<string> Authors { get; set; }
    }

    public class IndustryIdentifier
    {
        public string Type { get; set; }
        public string Identifier { get; set; }
    }

    public class ImageLinks
    {
        public string SmallThumbnail { get; set; }
        public string Thumbnail { get; set; }
    }
}
