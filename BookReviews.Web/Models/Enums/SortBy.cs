using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookReviews.Web.Models.Enums
{
    public enum SortBy : short
    {
        [Display(Name = "Oldest first", Order = 1)]
        DateAscending = 0,

        [Display(Name = "Newest first", Order = 0)]
        DateDescending = 1,

        [Display(Name = "Lowest rating first", Order = 3)]
        RatingAscending = 2,

        [Display(Name = "Highest rating first", Order = 2)]
        RatingDescending = 3
    }
}