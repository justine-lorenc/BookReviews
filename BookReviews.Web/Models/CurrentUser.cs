using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookReviews.Web.Models
{
    public class CurrentUser
    {
        public static bool IsLoggedIn()
        {
            bool isAuthenticated = HttpContext.Current?.User?.Identity?.IsAuthenticated ?? false;

            return isAuthenticated;
        }
    }
}