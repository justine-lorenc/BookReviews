using BookReviews.Impl.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book = BookReviews.Impl.Entities.Book;
using Author = BookReviews.Impl.Entities.Author;

namespace BookReviews.Impl.Repositories
{
    public class BookRepository : IBookRepository
    {
        public async Task<Book> GetBook(long bookId)
        {
            string query = @"SELECT TOP 1 [Id], [Title], [SubTitle], [Pages], [DatePublished], [Description], [CoverUrl]
                FROM [dbo].[Book] WHERE [Id] = @Id;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", bookId);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                Book book = await connection.QuerySingleOrDefaultAsync<Book>(query, parameters);
                return book;
            }
        }

        public async Task<Book> GetFullBook(long bookId)
        {
            string query = @"SELECT b.[Id], b.[Title], b.[SubTitle], b.[Pages], b.[DatePublished], b.[Description], b.[CoverUrl],
                a.[Id], a.[Name]
                FROM [dbo].[Book] AS b
                INNER JOIN [dbo].[BookAuthor] AS ba ON ba.[BookId] = b.[Id]
                INNER JOIN [dbo].[Author] AS a ON ba.[AuthorId] = a.[Id]
                WHERE b.[Id] = @Id;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", bookId);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                IEnumerable<Book> books = await connection.QueryAsync<Book, Author, Book>(
                    sql: query,
                    param: parameters,
                    map: (book, author) =>
                    {
                        if (book.Authors == null)
                            book.Authors = new List<Author>();

                        book.Authors.Add(author);
                        return book;
                    },
                    splitOn: "Id");

                books = books.GroupBy(b => b.Id).Select(g =>
                {
                    var book = g.First();
                    book.Authors = g.Select(b => b.Authors.Single()).ToList();
                    return book;
                });

                return books.FirstOrDefault();
            }
        }

        public async Task<List<Book>> GetFullBooks(List<long> bookIds)
        {
            string query = @"SELECT b.[Id], b.[Title], b.[SubTitle], b.[Pages], b.[DatePublished], b.[Description], b.[CoverUrl],
                a.[Id], a.[Name]
                FROM [dbo].[Book] AS b
                INNER JOIN [dbo].[BookAuthor] AS ba ON ba.[BookId] = b.[Id]
                INNER JOIN [dbo].[Author] AS a ON ba.[AuthorId] = a.[Id]
                WHERE b.[Id] IN @Ids;";

            var parameters = new DynamicParameters();
            parameters.Add("@Ids", bookIds);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                IEnumerable<Book> books = await connection.QueryAsync<Book, Author, Book>(
                    sql: query,
                    param: parameters,
                    map: (book, author) =>
                    {
                        if (book.Authors == null)
                            book.Authors = new List<Author>();

                        book.Authors.Add(author);
                        return book;
                    },
                    splitOn: "Id");

                books = books.GroupBy(b => b.Id).Select(g =>
                {
                    var book = g.First();
                    book.Authors = g.Select(b => b.Authors.FirstOrDefault()).ToList();
                    return book;
                });

                return books.ToList();
            }
        }

        public async Task<int> InsertBook(Book book)
        {
            string command = @"INSERT INTO [dbo].[Book] ([Id], [Title], [SubTitle], [Pages], [DatePublished], [Description], [CoverUrl])
                VALUES (@Id, @Title, @SubTitle, @Pages, @DatePublished, @Description, @CoverUrl);";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", book.Id);                 // this is not an identity column, it's the ISBN
            parameters.Add("@Title", book.Title);
            parameters.Add("@SubTitle", book.SubTitle);
            parameters.Add("@Pages", book.Pages);
            parameters.Add("@DatePublished", book.DatePublished);
            parameters.Add("@Description", book.Description);
            parameters.Add("@CoverUrl", book.CoverUrl);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                int booksInserted = await connection.ExecuteAsync(command, parameters);

                return booksInserted;
            }
        }
    }
}
