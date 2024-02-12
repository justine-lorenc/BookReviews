using BookReviews.Impl.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genre = BookReviews.Impl.Entities.Genre;

namespace BookReviews.Impl.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        public async Task<List<Genre>> GetGenres()
        {
            string query = @"SELECT [Id], [Name], [IsFiction] FROM [Genre];";

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                IEnumerable<Genre> genres = await connection.QueryAsync<Genre>(query);
                return genres.ToList();
            }
        }
    }
}
