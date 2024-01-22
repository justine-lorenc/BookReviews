using BookReviews.Impl.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookSearchResult = BookReviews.Impl.Entities.BookSearchResult;

namespace BookReviews.Impl.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<List<BookSearchResult>> SearchBooks(SearchCategory searchCategory, string searchTerm);
    }
}
