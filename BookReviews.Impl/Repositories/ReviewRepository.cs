using BookReviews.Impl.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Review = BookReviews.Impl.Entities.Review;
using Book = BookReviews.Impl.Entities.Book;
using Genre = BookReviews.Impl.Entities.Genre;
using User = BookReviews.Impl.Entities.User;

namespace BookReviews.Impl.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        public async Task<Review> GetReview(int reviewId)
        {
            string query = @"SELECT TOP 1 r.[Id], r.[Rating], r.[Notes], r.[DateAdded], r.[DateUpdated], r.[BookFormatId] AS [BookFormat],
                b.[Id],
                u.[Id], u.[FirstName], u.[LastName], 
                g.[Id], g.[Name], g.[IsFiction]
                FROM [dbo].[Review] AS r 
                INNER JOIN [dbo].[Book] AS b ON r.[BookId] = b.[Id]
                INNER JOIN [dbo].[User] AS u ON r.[UserId] = u.[Id]
                INNER JOIN [dbo].[Genre] AS g ON r.[GenreId] = g.[Id]
                WHERE r.[Id] = @Id;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", reviewId);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                IEnumerable<Review> reviews = await connection.QueryAsync<Review, Book, User, Genre, Review>(
                    sql: query,
                    param: parameters,
                    map: (review, book, user, genre) =>
                    {
                        review.Book = book;
                        review.Author = user;
                        review.Genre = genre;
                        return review;
                    },
                    splitOn: "Id, Id, Id");

                return reviews.FirstOrDefault();
            }
        }

        public async Task<List<Review>> GetReviews(long bookId)
        {
            string query = @"SELECT TOP 1 r.[Id], r.[Rating], r.[Notes], r.[DateAdded], r.[DateUpdated], r.[BookFormatId] AS [BookFormat],
                u.[Id], u.[FirstName], u.[LastName], 
                g.[Id], g.[Name], g.[IsFiction]
                FROM [dbo].[Review] AS r 
                INNER JOIN [dbo].[User] AS u ON r.[UserId] = u.[Id]
                INNER JOIN [dbo].[Genre] AS g ON r.[GenreId] = g.[Id]
                WHERE r.[BookId] = @BookId;";

            var parameters = new DynamicParameters();
            parameters.Add("@BookId", bookId);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                IEnumerable<Review> reviews = await connection.QueryAsync<Review, User, Genre, Review>(
                    sql: query,
                    param: parameters,
                    map: (review, user, genre) =>
                    {
                        review.Author = user;
                        review.Genre = genre;
                        return review;
                    },
                    splitOn: "Id, Id");

                return reviews.ToList();
            }
        }

        public async Task<int> InsertReview(Review review)
        {
            string command = @"INSERT INTO [dbo].[Review] ([Rating], [Notes], [DateAdded], [DateUpdated], [BookId], [UserId], [GenreId], [BookFormatId])
            VALUES (@Rating, @Notes, @DateAdded, @DateUpdated, @BookId, @UserId, @GenreId, @BookFormatId);
            SELECT SCOPE_IDENTITY();";

            var now = DateTime.Now;

            var parameters = new DynamicParameters();
            parameters.Add("@Rating", review.Rating);
            parameters.Add("@Notes", review.Notes);
            parameters.Add("@DateAdded", now);
            parameters.Add("@DateUpdated", now);
            parameters.Add("@BookId", review.Book.Id);
            parameters.Add("@UserId", review.Author.Id);
            parameters.Add("@GenreId", review.Genre.Id);
            parameters.Add("@BookFormatId", (short)review.BookFormat);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                int reviewId = await connection.ExecuteScalarAsync<int>(command, parameters);

                return reviewId;
            }
        }

        public async Task<int> UpdateReview(Review review)
        {
            string command = @"UPDATE [dbo].[Review] SET
                [Rating] = @Rating, 
                [Notes] = @Notes,
                [DateUpdated] = @DateUpdated, 
                [GenreId] = @GenreId,
                [BookFormatId] = @BookFormatId
            WHERE [Id] = @Id;";

            var now = DateTime.Now;

            var parameters = new DynamicParameters();
            parameters.Add("@Id", review.Id);
            parameters.Add("@Rating", review.Rating);
            parameters.Add("@Notes", review.Notes);
            parameters.Add("@DateUpdated", now);
            parameters.Add("@GenreId", review.Genre.Id);
            parameters.Add("@BookFormatId", (short)review.BookFormat);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                int reviewsUpdated = await connection.ExecuteAsync(command, parameters);

                return reviewsUpdated;
            }
        }

        public async Task<int> DeleteReview(int reviewId)
        {
            string command = @"DELETE FROM [dbo].[Review] WHERE [Id] = @Id;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", reviewId);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                int reviewsDeleted = await connection.ExecuteAsync(command, parameters);

                return reviewsDeleted;
            }
        }
    }
}
