using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = BookReviews.Impl.Entities.User;
using Role = BookReviews.Impl.Entities.Enums.Role;

namespace BookReviews.Impl.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddUser(User user);
        Task<User> GetUser(string emailAddress);
        Task<List<Role>> GetUserRoles(int userId);
    }
}
