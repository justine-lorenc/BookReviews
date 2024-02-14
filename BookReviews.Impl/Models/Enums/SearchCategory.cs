using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Models.Enums
{
    public enum SearchCategory : short
    {
        [Display(Name = "Title")]
        Title = 0,

        [Display(Name = "Author")]
        Author = 1,

        [Display(Name = "ISBN")]
        Isbn = 2
    }
}
