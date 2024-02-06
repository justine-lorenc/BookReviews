using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Logic.Interfaces
{
    public interface IUserLogic
    {
        Task<bool> RegisterAccount(Models.User user, string password);
        Task<Models.User> AuthenticateUser(string username, string password);
        Task<List<Models.Enums.Role>> GetUserRoles(int userId);
    }
}
