using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book = BookReviews.Impl.Entities.Book;

namespace BookReviews.Impl.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> GetBook(long bookId);
        Task<Book> GetFullBook(long bookId);
        Task<int> InsertBook(Book book);
    }
}
