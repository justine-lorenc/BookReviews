using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exception = BookReviews.Impl.Entities.Exception;

namespace BookReviews.Impl.Repositories.Interfaces
{
    public interface IExceptionRepository
    {
        Task<Guid> InsertException(Exception error);
    }
}
