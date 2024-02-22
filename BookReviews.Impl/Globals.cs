using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl
{
    public static class Globals
    {
        public static class ConnectionStrings
        {
            public static string BookReviewsDB = ConfigurationManager.ConnectionStrings["BookReviewsDB"].ToString();
        }

        public static class AppSettings
        {
            public static string BookSearchClientName = "BookSearch";
            public static string GoogleBooksApiUrl = ConfigurationManager.AppSettings["GoogleBooksApiUrl"].ToString();
            public static string GoogleBooksApiKey = ConfigurationManager.AppSettings["GoogleBooksApiKey"].ToString();
            public static string HashSalt = ConfigurationManager.AppSettings["HashSalt"].ToString();
            public static int AuthTicketExpiration = Convert.ToInt32(ConfigurationManager.AppSettings["AuthTicketExpiration"]);
        }

        public static class RegularExpressions
        {
            public const string EmailAddress = @"[a-zA-Z0-9-_.]+@[a-zA-Z0-9-_.]+\.[a-zA-Z]+";
            public const string Password = @"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%^&*-_=+]).{10,}";
            public const string FirstName = @"[a-zA-Z-'\s]+";
            public const string LastName = @"[a-zA-Z-'\s]+";
        }

        public static class MaxLengths
        {
            public const int SearchTerm = 100;

            public static class Author
            {
                public const int Name = 200;
            }

            public static class Book
            {
                public const int Id = 13;
                public const int Title = 250;
                public const int SubTitle = 250;
                public const int Description = 8000;
                public const int CoverUrl = 1000;
            }

            public static class Review
            {
                public const int Notes = 4000;
            }
            
            public static class User
            {
                public const int EmailAddress = 100;
                public const int Password = 50;
                public const int FirstName = 50;
                public const int LastName = 50;
            }
        }

        public static class Ranges
        {
            public static class Rating
            {
                public const float Minimum = 0.0F;
                public const float Maximum = 5.0F;
                public const float Increment = 0.5F;
            }
        }
    }
}
