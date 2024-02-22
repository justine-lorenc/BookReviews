using AutoMapper;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Web.Models.Enums;
using BookReviews.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookReviews.Impl.Models;
using System.Threading.Tasks;
using BookReviews.Web.Models;
using BookReviews.Web.Filters;
using BookReviews.Impl.Models.Enums;
using BookReviews.Impl;

namespace BookReviews.Web.Controllers
{
    [RoutePrefix("reviews")]
    public class ReviewController : BaseController
    {
        private IMapper _mapper;
        private IBookLogic _bookLogic;
        private IReviewLogic _reviewLogic;

        public ReviewController(IMapper mapper, IBookLogic bookLogic, IExceptionLogic exceptionLogic, IReviewLogic reviewLogic)
            : base(exceptionLogic)
        {
            _mapper = mapper;
            _bookLogic = bookLogic;
            _reviewLogic = reviewLogic;
        }

        [HttpGet]
        [Route("~/users/{id:int}/reviews")]
        public async Task<ActionResult> UserReviews(int id, int year = 0)
        {
            if (id == 0)
                throw new Exception("User ID is invalid");
            else if (id != CurrentUser.Id && !CurrentUser.HasAbility(Role.Admin))
                return RedirectToAction("Forbidden", "Home");

            int currentYear = DateTime.Today.Year;

            // if the year is invalid, set it to the current year (0 equals all reviews)
            if (year != 0 && (year < 2020 || year > currentYear))
            {
                year = currentYear;
            }

            List<int> reviewYears = await _reviewLogic.GetReviewYears(id);
            reviewYears = reviewYears.OrderByDescending(x => x).Prepend(0).ToList();

            Dictionary<int, string> reviewYearsSelect = reviewYears.ToDictionary(k => k, v => v.ToString());
            reviewYearsSelect[0] = "All";

            var model = new UserReviews()
            {
                UserId = id,
                Year = year,
                Years = reviewYearsSelect
            };

            // retrieve reviews
            List<Review> userReviews = await _reviewLogic.GetReviews(id, true, year);

            // calculate stats
            if (userReviews != null && userReviews.Count > 0)
            {           
                // overview
                int totalBooks = userReviews.Count;
                int totalPages = userReviews.Sum(x => x.Book.Pages);
                decimal ratingSum = userReviews.Sum(x => (decimal)x.Rating);
                decimal averageRating = totalBooks == 0 ? 0 : (ratingSum / (decimal)totalBooks);

                // format stats
                List<BookFormat> formats = userReviews.GroupBy(x => x.BookFormat)
                    .Select(g => g.First().BookFormat).ToList();

                var formatsRead = new List<FormatRead>();
                foreach (var format in formats)
                {
                    int formatCount = userReviews.Where(x => x.BookFormat == format).Count();
                    decimal formatPercent = CalculatePercent((decimal)formatCount, (decimal)totalBooks);

                    var formatRead = new FormatRead()
                    {
                        Format = format,
                        Count = formatCount,
                        Percent = formatPercent
                    };

                    formatsRead.Add(formatRead);
                }

                // genre stats
                List<Genre> genres = userReviews.GroupBy(x => x.Genre.Id).Select(g => g.First().Genre).ToList();

                int totalFiction = userReviews.Where(x => x.Genre.IsFiction == true).Count();
                decimal fictionPercent = CalculatePercent((decimal)totalFiction, (decimal)totalBooks);

                int totalNonfiction = userReviews.Where(x => x.Genre.IsFiction == false).Count();
                decimal nonfictionPercent = CalculatePercent((decimal)totalNonfiction, (decimal)totalBooks);

                var genresRead = new List<GenreRead>();
                foreach (var genre in genres)
                {
                    int genreCount = userReviews.Where(x => x.Genre.Id == genre.Id).Count();
                    decimal genrePercent = CalculatePercent((decimal)genreCount, (decimal)totalBooks);

                    var genreRead = new GenreRead()
                    {
                        Id = genre.Id,
                        Name = genre.Name,
                        Count = genreCount,
                        Percent = genrePercent
                    };

                    genresRead.Add(genreRead);
                }

                model.TotalBooks = totalBooks;
                model.TotalPages = totalPages;
                model.AverageRating = averageRating;
                model.FormatsRead = formatsRead.OrderByDescending(x => x.Count).ToList();
                model.TotalFiction = totalFiction;
                model.FictionPercent = fictionPercent;
                model.TotalNonfiction = totalNonfiction;
                model.NonfictionPercent = nonfictionPercent;
                model.GenresRead = genresRead.OrderByDescending(x => x.Count).ToList();
                model.Reviews = userReviews.OrderByDescending(x => x.DateUpdated).ToList();
            }

            return View(model);
        }
        

        [HttpGet]
        [Route("add")]
        [HasAbility(Role.AddReview)]
        public async Task<ActionResult> AddReview(long bookId)
        { 
            var model = new AddReviewVM();
            model = await PopulateFormFields(bookId, model);

            return View(model);
        }

        [HttpPost]
        [Route("add")]
        [HasAbility(Role.AddReview)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddReview(AddReviewVM model)
        {
            if ((model.Rating % Globals.Ranges.Rating.Increment) != 0)
                ModelState.AddModelError("Rating", $"Rating must be a multiple of 0.5");

            if (!ModelState.IsValid)
                return View(model);

            model.UserId = CurrentUser.Id;
            Review review = _mapper.Map<Review>(model);
            bool result = await _reviewLogic.AddReview(review);

            if (!result)
            {
                SetResultMessage(MessageType.Error, "Failed to submit review");
                return View(model);
            }

            SetResultMessage(MessageType.Success, "Review submitted");
            return RedirectToAction("Index", "Book", new { id = model.Book.Id });
        }

        [HttpGet]
        [Route("{id:int}/edit")]
        [HasAbility(Role.EditReview)]
        public async Task<ActionResult> EditReview(int id)
        {
            var model = new EditReviewVM();
            model = await PopulateFormFields(id, model);

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Put)]
        [Route("{id:int}/edit")]
        [HasAbility(Role.EditReview)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditReview(int id, EditReviewVM model)
        {
            if ((model.Rating % Globals.Ranges.Rating.Increment) != 0)
                ModelState.AddModelError("Rating", $"Rating must be a multiple of 0.5");

            if (!ModelState.IsValid)
                return View(model);

            Review review = _mapper.Map<Review>(model);
            bool result = await _reviewLogic.EditReview(review);

            if (!result)
            {
                SetResultMessage(MessageType.Error, "Failed to update review");
                return View(model);
            }

            SetResultMessage(MessageType.Success, "Review updated");
            return RedirectToAction("Index", "Book", new { id = model.Book.Id });
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Delete)]
        [Route("{id:int}/delete")]
        [HasAbility(Role.DeleteReview)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteReview(int id, long bookId)
        {
            if (id == 0)
            {
                return Json(new { success = false });
            }

            bool result = await _reviewLogic.DeleteReview(id);

            if (!result)
            {
                return Json(new { success = false });
            }

            SetResultMessage(MessageType.Success, "Review deleted");
            return Json(new { success = true });
        }

        private async Task<AddReviewVM> PopulateFormFields(long bookId, AddReviewVM model)
        {
            Book book = await _bookLogic.GetBook(bookId);
            model.Book = book;

            List<Genre> genres = await _reviewLogic.GetGenres();
            model.Genres = genres;

            return model;
        }

        private async Task<EditReviewVM> PopulateFormFields(int reviewId, EditReviewVM model)
        {
            Review review = await _reviewLogic.GetReview(reviewId);
            model = _mapper.Map<EditReviewVM>(review);

            List<Genre> genres = await _reviewLogic.GetGenres();
            model.Genres = genres;

            return model;
        }

        private decimal CalculatePercent(decimal dividend, decimal divisor)
        {
            if (divisor == 0)
                return 0.0M;

            decimal result = (dividend / divisor) * 100;
            result = Math.Round(result, 2);

            return result;
        }
    }
}