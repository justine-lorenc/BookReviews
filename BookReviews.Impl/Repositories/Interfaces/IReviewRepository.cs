using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Review = BookReviews.Impl.Entities.Review;

namespace BookReviews.Impl.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> GetReview(int reviewId);
        Task<List<Review>> GetReviews(long bookId);
        Task<int> InsertReview(Review review);
        Task<int> UpdateReview(Review review);
        Task<int> DeleteReview(int reviewId);
    }
}
