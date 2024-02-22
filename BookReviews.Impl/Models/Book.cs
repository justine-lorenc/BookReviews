using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Models
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

        public bool IsValid(out string errorMessage)
        {
            errorMessage = String.Empty;

            if (Id == 0 || Id.ToString().Length > Globals.MaxLengths.Book.Id)
            {
                errorMessage = $"ID is invalid: {Id}";
                return false;
            }
            if (String.IsNullOrWhiteSpace(Title))
            {
                errorMessage = "Title is null or empty";
                return false;
            }
            else if (Title.Length > Globals.MaxLengths.Book.Title)
            {
                errorMessage = "Title exceeds max length";
                return false;
            }
            else if (!String.IsNullOrWhiteSpace(SubTitle) && SubTitle.Length > Globals.MaxLengths.Book.SubTitle)
            {
                errorMessage = "Subtitle exceeds max length";
                return false;
            }
            else if (Pages <= 0)
            {
                errorMessage = $"Page count is invalid: {Pages}";
                return false;
            }
            else if (DatePublished == DateTime.MinValue || DatePublished >= DateTime.Today.AddDays(1))
            {
                errorMessage = $"Date published is outside acceptable range: {DatePublished}";
                return false;
            }
            else if (!String.IsNullOrWhiteSpace(Description) && Description.Length > Globals.MaxLengths.Book.Description)
            {
                errorMessage = "Description exceeds max length";
                return false;
            }
            else if (!String.IsNullOrWhiteSpace(CoverUrl) && CoverUrl.Length > Globals.MaxLengths.Book.CoverUrl)
            {
                errorMessage = "Cover URL exceeds max length";
                return false;
            }
            else if (Authors == null || Authors.Count == 0)
            {
                errorMessage = "Author list is null or empty";
                return false;
            }

            return true;
        }
    }
}
