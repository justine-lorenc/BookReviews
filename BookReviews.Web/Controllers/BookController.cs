using AutoMapper;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Impl.Models;
using BookReviews.Impl.Models.Enums;
using BookReviews.Web.Models;
using BookReviews.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Book = BookReviews.Impl.Models.Book;
using BookReviews.Web.Models.Enums;

namespace BookReviews.Web.Controllers
{
    [RoutePrefix("books")]
    public class BookController : BaseController
    {
        private IMapper _mapper;
        private IBookLogic _bookLogic;
        private IReviewLogic _reviewLogic;

        public BookController(IMapper mapper, IBookLogic bookLogic, IExceptionLogic exceptionLogic, IReviewLogic reviewLogic)
            : base(exceptionLogic)
        {
            _mapper = mapper;
            _bookLogic = bookLogic;
            _reviewLogic = reviewLogic;
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<ActionResult> Index(long id, SortBy sortReviewsBy = SortBy.DateDescending)
        {
            if (id == 0)
                throw new Exception("Book ID is invalid");

            Book book = await _bookLogic.GetBook(id);

            ReviewInfo reviewInfo = await GetReviewInfo(id, sortReviewsBy);

            var model = new BookInfo()
            {
                Book = book,
                Reviews = reviewInfo
            };

            return View(model);
        }

        [HttpGet]
        [Route("{id:long}/reviews")]
        public async Task<ActionResult> GetReviews(long id, SortBy sortBy = SortBy.DateDescending)
        {
            ReviewInfo model = await GetReviewInfo(id, sortBy);

            return PartialView("~/Views/Review/_BookReviews.cshtml", model);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult> Search(SearchCategory searchCategory = SearchCategory.Title, string searchTerm = null)
        {
            if (String.IsNullOrWhiteSpace(searchTerm))
                return View();

            var model = new SearchVM()
            {
                SearchCategory = searchCategory,
                SearchTerm = searchTerm
            };

            // sanitize search term
            searchTerm = searchTerm.Trim();

            // initialize results list
            List<Book> results = await _bookLogic.SearchBooks(searchCategory, searchTerm);

            // group books with the same title and author(s), then display the earliest published
            // edition from each group
            results = results.GroupBy(b => 
                new { 
                    b.Title, 
                    Author = b.Authors.OrderBy(x => x.Name).Select(x => x.Name).FirstOrDefault() 
                })
                .Select(g => g.OrderBy(b => b.DatePublished).First()).ToList();

            // TODO: pagination may be needed
            model.Results = results;

            return View(model);
        }

        private async Task<ReviewInfo> GetReviewInfo(long bookId, SortBy sortBy)
        {
            List<Review> reviews = await _reviewLogic.GetReviews(bookId);

            // calculate stats
            int totalReviews = reviews.Count;
            decimal ratingSum = reviews.Select(x => (decimal)x.Rating).Sum();
            decimal averageRating = totalReviews == 0 ? 0 : (ratingSum / (decimal)totalReviews);
            averageRating = Math.Round(averageRating, 2);

            // find current user's review
            Review currentUserReview = reviews.Where(x => x.User.Id == CurrentUser.Id).FirstOrDefault();

            if (currentUserReview != null)
                reviews.Remove(currentUserReview);

            // sort community reviews
            switch (sortBy)
            {
                case SortBy.DateAscending:
                    reviews = reviews.OrderBy(x => x.DateAdded).ToList();
                    break;
                case SortBy.DateDescending:
                    reviews = reviews.OrderByDescending(x => x.DateAdded).ToList();
                    break;
                case SortBy.RatingAscending:
                    reviews = reviews.OrderBy(x => x.Rating).ToList();
                    break;
                case SortBy.RatingDescending:
                    reviews = reviews.OrderByDescending(x => x.Rating).ToList();
                    break;
            }

            var reviewInfo = new ReviewInfo()
            {
                BookId = bookId,
                TotalReviews = totalReviews,
                AverageRating = averageRating,
                UserReview = currentUserReview,
                CommunityReviews = reviews
            };

            return reviewInfo;
        }
    }
}