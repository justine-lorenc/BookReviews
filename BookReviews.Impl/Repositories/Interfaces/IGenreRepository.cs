using BookReviews.Impl.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genre = BookReviews.Impl.Entities.Genre;

namespace BookReviews.Impl.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetGenres();
    }
}