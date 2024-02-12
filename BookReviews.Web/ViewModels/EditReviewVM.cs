using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookReviews.Web.ViewModels
{
    public class EditReviewVM : AddReviewVM
    {
        public int ReviewId { get; set; }
    }
}