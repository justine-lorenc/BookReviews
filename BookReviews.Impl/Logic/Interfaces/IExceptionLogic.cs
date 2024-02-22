using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Logic.Interfaces
{
    public interface IExceptionLogic
    {
        Task<Guid> LogException(System.Exception exception, string message, Dictionary<string, string> arguments = null, int userId = 0,
            [CallerMemberName] string method = null, [CallerFilePath] string filePath = null);
        Task<Guid> LogException(string error, string message, Dictionary<string, string> arguments = null, int userId = 0,
            [CallerMemberName] string method = null, [CallerFilePath] string filePath = null);
    }
}
