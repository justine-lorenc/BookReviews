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

namespace BookReviews.Web.Controllers
{
    [RoutePrefix("reviews")]
    public class ReviewController : BaseController
    {
        private IMapper _mapper;
        private IBookLogic _bookLogic;
        private IReviewLogic _reviewLogic;

        public ReviewController(IMapper mapper, IBookLogic bookLogic, IReviewLogic reviewLogic)
        {
            _mapper = mapper;
            _bookLogic = bookLogic;
            _reviewLogic = reviewLogic;
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
        public async Task<JsonResult> DeleteReview(int id, long bookId)
        {
            if (id == 0)
            {
                SetResultMessage(MessageType.Error, "Failed to delete review");
                return Json(new { success = false });
            }

            bool result = await _reviewLogic.DeleteReview(id);

            if (!result)
            {
                SetResultMessage(MessageType.Error, "Failed to delete review");
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
    }
}