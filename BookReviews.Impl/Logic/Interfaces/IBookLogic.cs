using BookReviews.Impl.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Logic.Interfaces
{
    public interface IBookLogic
    {
        Task<List<Models.Book>> SearchBooks(SearchCategory searchCategory, string searchTerm);
    }
}
