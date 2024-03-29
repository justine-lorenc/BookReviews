﻿using AutoMapper;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Impl.Repositories;
using BookReviews.Impl.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace BookReviews.Impl.Logic
{
    public class ReviewLogic : IReviewLogic
    {
        private IMapper _mapper;
        private IBookLogic _bookLogic;
        private IExceptionLogic _exceptionLogic;
        private IGenreRepository _genreRepository;
        private IReviewRepository _reviewRepository;

        public ReviewLogic(IMapper mapper, IBookLogic bookLogic, IExceptionLogic exceptionLogic, 
            IGenreRepository genreRepository, IReviewRepository reviewRepository)
        {
            _mapper = mapper;
            _bookLogic = bookLogic;
            _exceptionLogic = exceptionLogic;
            _genreRepository = genreRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<Models.Review> GetReview(int reviewId, bool includeBook = true)
        {
            try
            {
                if (reviewId == 0)
                    throw new Exception("Review ID is invalid");

                Entities.Review reviewRecord = await _reviewRepository.GetReview(reviewId);
                Models.Review reviewResult = _mapper.Map<Models.Review>(reviewRecord);

                if (includeBook)
                {
                    Models.Book bookRecord = await _bookLogic.GetBook(reviewRecord.Book.Id);

                    if (bookRecord == null)
                        throw new Exception("Failed to retrieve book");

                    reviewResult.Book = bookRecord;
                }

                return reviewResult;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                arguments.Add("ReviewId", reviewId.ToString());
                arguments.Add("IncludeBook", includeBook.ToString());

                await _exceptionLogic.LogException(ex, "Get review error", arguments);
                
                return null;
            }
        }

        public async Task<List<Models.Review>> GetReviews(long bookId)
        {
            try
            {
                if (bookId == 0 || bookId.ToString().Length < Globals.MaxLengths.Book.Id)
                    throw new Exception("Book ID is invalid");

                List<Entities.Review> reviewRecords = await _reviewRepository.GetReviews(bookId);
                List<Models.Review> reviewResults = _mapper.Map<List<Models.Review>>(reviewRecords);

                return reviewResults;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                arguments.Add("BookId", bookId.ToString());

                await _exceptionLogic.LogException(ex, "Get reviews for book error", arguments);

                return new List<Models.Review>();
            }
        }

        public async Task<List<Models.Review>> GetReviews(int userId, bool includeBooks = true, int year = 0)
        {
            try
            {
                if (userId == 0)
                    throw new Exception("User ID is invalid");

                DateTime? startDate = null;
                DateTime? endDate = null;

                if (year > 0)
                {
                    startDate = new DateTime(year, 1, 1);
                    endDate = startDate.Value.AddYears(1);
                }

                List<Entities.Review> reviewRecords = await _reviewRepository.GetReviews(userId, startDate, endDate);
                List<Models.Review> reviewResults = _mapper.Map<List<Models.Review>>(reviewRecords);

                if (includeBooks)
                {
                    List<long> bookIds = reviewResults.Select(x => x.Book.Id).ToList();
                    List<Models.Book> bookResults = await _bookLogic.GetBooks(bookIds);

                    foreach (var reviewResult in reviewResults)
                    {
                        Models.Book bookResult = bookResults.Where(x => x.Id == reviewResult.Book.Id).FirstOrDefault();

                        if (bookResult == null)
                            throw new Exception("Failed to retrieve review's book");

                        reviewResult.Book = bookResult;
                    }
                }

                return reviewResults;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                arguments.Add("UserId", userId.ToString());
                arguments.Add("IncludeBooks", includeBooks.ToString());
                arguments.Add("Year", year.ToString());

                await _exceptionLogic.LogException(ex, "Get reviews for user error", arguments);

                return new List<Models.Review>();
            }
        }

        public async Task<List<int>> GetReviewYears(int userId)
        {
            try
            {
                if (userId == 0)
                    throw new Exception("User ID is invalid");

                List<DateTime> reviewDateRecords = await _reviewRepository.GetReviewDates(userId);
                List<int> reviewYears = reviewDateRecords.Select(x => x.Year).Distinct().ToList();

                return reviewYears;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                arguments.Add("UserId", userId.ToString());

                await _exceptionLogic.LogException(ex, "Get user review years error", arguments);

                return new List<int>();
            }
        }

        public async Task<bool> AddReview(Models.Review review)
        {
            try
            {
                if (review == null)
                    throw new Exception("Review is null");
                else if (!review.IsValid(out string errorMessage))
                    throw new Exception(errorMessage);

                // first insert the book info if it doesn't exist in the database
                bool bookInserted = await _bookLogic.AddBook(review.Book);

                if (bookInserted == false)
                    throw new Exception("Failed to insert book");

                Entities.Review reviewRecord = _mapper.Map<Entities.Review>(review);
                int reviewId = await _reviewRepository.InsertReview(reviewRecord);

                if (reviewId == 0)
                    throw new Exception("Failed to insert review");

                return true;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                if (review != null)
                {
                    arguments.Add("BookId", review.Book?.Id.ToString());
                    arguments.Add("UserId", review.User?.Id.ToString());
                }

                await _exceptionLogic.LogException(ex, "Add review error", arguments);

                return false;
            }
        }

        public async Task<bool> EditReview(Models.Review review)
        {
            try
            {
                // this method assumes that the associated book was successfully inserted
                // when the review was first written
                if (review == null)
                    throw new Exception("Review is null");
                else if (!review.IsValid(out string errorMessage))
                    throw new Exception(errorMessage);

                Entities.Review reviewRecord = _mapper.Map<Entities.Review>(review);
                int recordsUpdated = await _reviewRepository.UpdateReview(reviewRecord);

                if (recordsUpdated == 0)
                    throw new Exception("Failed to update review");

                return true;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                if (review != null)
                {
                    arguments.Add("ReviewId", review.Id.ToString());
                    arguments.Add("BookId", review.Book?.Id.ToString());
                    arguments.Add("UserId", review.User?.Id.ToString());
                }

                await _exceptionLogic.LogException(ex, "Edit review error", arguments);

                return false;
            }
        }

        public async Task<bool> DeleteReview(int reviewId)
        {
            try
            {
                if (reviewId == 0)
                    throw new Exception("Review ID is invalid");

                int recordsDeleted = await _reviewRepository.DeleteReview(reviewId);

                if (recordsDeleted == 0)
                    throw new Exception("Failed to delete review");

                return true;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                arguments.Add("ReviewId", reviewId.ToString());

                await _exceptionLogic.LogException(ex, "Delete review error", arguments);

                return false;
            }
        }

        public async Task<List<Models.Genre>> GetGenres()
        {
            try
            {
                List<Entities.Genre> genreRecords = await _genreRepository.GetGenres();
                List<Models.Genre> genreResults = _mapper.Map<List<Models.Genre>>(genreRecords);

                return genreResults;
            }
            catch (Exception ex)
            {
                await _exceptionLogic.LogException(ex, "Get genres error");

                return new List<Models.Genre>();
            }
        }
    }
}
