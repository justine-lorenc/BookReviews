using BookReviews.Impl.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Models
{
    public class Review
    {
        public int Id { get; set; }
        public float Rating { get; set; }
        public string Notes { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public Genre Genre { get; set; }
        public BookFormat BookFormat { get; set; }

        public bool IsValid(out string errorMessage)
        {
            errorMessage = String.Empty;

            if (Rating < Globals.Ranges.Rating.Minimum || Rating > Globals.Ranges.Rating.Maximum)
            {
                errorMessage = $"Rating is outside acceptable range: {Rating}";
                return false;
            }
            else if ((Rating % Globals.Ranges.Rating.Increment) != 0)
            {
                errorMessage = $"Rating must be a multiple of 0.5: {Rating}";
                return false;
            }
            else if (Book == null)
            {
                errorMessage = "Book is null";
                return false;
            }
            else if (Book.Id == 0 || Book.Id.ToString().Length > Globals.MaxLengths.Book.Id)
            {
                errorMessage = $"Book ID is invalid: {Book.Id}";
                return false;
            }
            else if (User == null)
            {
                errorMessage = "User is null";
                return false;
            }
            else if (User.Id == 0)
            {
                errorMessage = "User ID is invalid";
                return false;
            }
            else if (Genre == null)
            {
                errorMessage = "Genre is null";
                return false;
            }
            else if (Genre.Id == 0)
            {
                errorMessage = "Genre ID is invalid";
                return false;
            }
            else if (!Enum.IsDefined(typeof(BookFormat), BookFormat))
            {
                errorMessage = $"Book format is undefined: {(short)BookFormat}";
                return false;
            }

            return true;
        }
    }
}
