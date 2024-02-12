using BookReviews.Impl.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = BookReviews.Impl.Entities.User;
using Role = BookReviews.Impl.Entities.Enums.Role;

namespace BookReviews.Impl.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetUser(string emailAddress)
        {
            string query = @"SELECT TOP 1 [Id], [FirstName], [LastName], [DateAdded], [DateUpdated], [EmailAddress], [PasswordHash], [IsActive]
                FROM [dbo].[User] WHERE [EmailAddress] = @EmailAddress;";

            var parameters = new DynamicParameters();
            parameters.Add("@EmailAddress", emailAddress);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                User user = await connection.QuerySingleOrDefaultAsync<User>(query, parameters);

                return user;
            }
        }

        public async Task<int> InsertUser(User user)
        {
            string command = @"INSERT INTO [dbo].[User] ([FirstName], [LastName], [EmailAddress], [PasswordHash], [DateAdded], [DateUpdated], [IsActive]) 
                VALUES (@FirstName, @LastName, @EmailAddress, @PasswordHash, @DateAdded, @DateUpdated, @IsActive); 
                SELECT SCOPE_IDENTITY();"; 
            
            var now = DateTime.Now;

            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", user.FirstName);
            parameters.Add("@LastName", user.LastName);
            parameters.Add("@EmailAddress", user.EmailAddress);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@DateAdded", now);
            parameters.Add("@DateUpdated", now);
            parameters.Add("@IsActive", 0);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                int userId = await connection.ExecuteScalarAsync<int>(command, parameters);

                return userId;
            }
        }

        public async Task<List<Role>> GetUserRoles(int userId)
        {
            string query = @"SELECT [RoleId] FROM [dbo].[UserRole] WHERE [UserId] = @UserId;";

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            using (var connection = new SqlConnection(Globals.ConnectionStrings.BookReviewsDB))
            {
                IEnumerable<Role> roles = await connection.QueryAsync<Role>(query, parameters);
                return roles.ToList();
            }
        }
    }
}
