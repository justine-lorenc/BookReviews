using BookReviews.Impl.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Author = BookReviews.Impl.Entities.Author;

namespace BookReviews.Impl.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        public async Task<List<Author>> GetAuthors(List<string> names)
        {
            string query = @"SELECT [Id], [Name] FROM [dbo].[Author] WHERE [Name] IN @Names;";

            var parameters = new DynamicParameters();
            parameters.Add("@Names", names);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                IEnumerable<Author> authors = await connection.QueryAsync<Author>(query, parameters);
                return authors.ToList();
            }
        }

        public async Task<int> InsertAuthor(Author author)
        {
            string command = @"INSERT INTO [dbo].[Author] ([Name]) VALUES (@Name);
                SELECT SCOPE_IDENTITY();";

            var parameters = new DynamicParameters();
            parameters.Add("@Name", author.Name);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                int authorId = await connection.ExecuteScalarAsync<int>(command, parameters);
                return authorId;
            }
        }

        public async Task<int> InsertBookAuthors(long bookId, List<int> authorIds)
        {
            string command = @"INSERT INTO [dbo].[BookAuthor] ([BookId], [AuthorId])
                SELECT [BookId], [AuthorId] FROM @BookAuthors;";

            var dataTable = new DataTable("[dbo].[TVP_BookAuthor]");
            dataTable.Columns.Add("BookId", typeof(long));
            dataTable.Columns.Add("AuthorId", typeof(int));

            foreach (var authorId in authorIds)
            {
                var row = dataTable.NewRow();
                row["BookId"] = bookId;
                row["AuthorId"] = authorId;

                dataTable.Rows.Add(row);
            }

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                int bookAuthorsInserted = await connection.ExecuteAsync(command, 
                    new { BookAuthors = dataTable.AsTableValuedParameter("[dbo].[TVP_BookAuthor]") });

                return bookAuthorsInserted;
            }
        }
    }
}
