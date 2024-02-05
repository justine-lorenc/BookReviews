using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = BookReviews.Impl.Entities.User;

namespace BookReviews.Impl.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddUser(User user);
        Task<User> GetUser(string emailAddress);
    }
}
