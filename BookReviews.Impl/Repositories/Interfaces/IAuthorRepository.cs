using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Author = BookReviews.Impl.Entities.Author;

namespace BookReviews.Impl.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author> GetAuthor(string name);
        Task<List<Author>> GetAuthors(List<string> names);
        Task<List<Author>> GetAuthors(long bookId);
        Task<int> InsertAuthor(Author author);
        Task<int> InsertBookAuthors(long bookId, List<int> authorIds);
    }
}
