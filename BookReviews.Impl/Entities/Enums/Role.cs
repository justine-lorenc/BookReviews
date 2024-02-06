using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Entities.Enums
{
    public enum Role : short
    {
        Admin = 0,
        AddReview = 1,
        EditReview = 2,
        DeleteReview = 3
    }
}
