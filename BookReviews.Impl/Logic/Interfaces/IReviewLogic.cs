using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Logic.Interfaces
{
    public interface IReviewLogic
    {
        Task<Models.Review> GetReview(int reviewId, bool includeBook = true);
        Task<List<Models.Review>> GetReviews(long bookId);
        Task<List<Models.Review>> GetReviews(int userId, bool includeBooks = true, int year = 0);
        Task<List<int>> GetReviewYears(int userId);
        Task<bool> AddReview(Models.Review review);
        Task<bool> EditReview(Models.Review review);
        Task<bool> DeleteReview(int reviewId);
        Task<List<Models.Genre>> GetGenres();
    }
}
