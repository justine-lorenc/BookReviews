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
        Task<Models.Book> GetBook(long id);
        Task<List<Models.Book>> GetBooks(List<long> ids);
        Task<List<Models.Book>> SearchBooks(SearchCategory searchCategory, string searchTerm);
        Task<bool> AddBook(Models.Book book);
    }
}
