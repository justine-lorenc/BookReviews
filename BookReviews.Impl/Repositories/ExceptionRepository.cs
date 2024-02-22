using BookReviews.Impl.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exception = BookReviews.Impl.Entities.Exception;

namespace BookReviews.Impl.Repositories
{
    public class ExceptionRepository : IExceptionRepository
    {
        public async Task<Guid> InsertException(Exception exception)
        {
            string command = @"INSERT INTO [dbo].[Exception] ([Error], [StackTrace], [DateAdded], [Message], [Method], [Arguments], [UserId])
                OUTPUT INSERTED.[Id]                
                VALUES (@Error, @StackTrace, @DateAdded, @Message, @Method, @Arguments, @UserId);";

            DateTime now = DateTime.Now;

            var parameters = new DynamicParameters();
            parameters.Add("@Error", exception.Error);
            parameters.Add("@StackTrace", exception.StackTrace);
            parameters.Add("@DateAdded", now);
            parameters.Add("@Message", exception.Message);
            parameters.Add("@Method", exception.Method);
            parameters.Add("@Arguments", exception.Arguments);
            parameters.Add("@UserId", exception.UserId);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                Guid exceptionId = await connection.ExecuteScalarAsync<Guid>(command, parameters);

                return exceptionId;
            }
        }
    }
}
