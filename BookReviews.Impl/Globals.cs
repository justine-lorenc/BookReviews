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
    }
}
