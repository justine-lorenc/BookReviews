using AutoMapper;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Impl.Models.Enums;
using BookReviews.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Book = BookReviews.Impl.Models.Book;

namespace BookReviews.Web.Controllers
{
    [RoutePrefix("books")]
    public class BookController : Controller
    {
        private IMapper _mapper;
        private IBookLogic _bookLogic;

        public BookController(IMapper mapper, IBookLogic bookLogic)
        {
            _mapper = mapper;
            _bookLogic = bookLogic;
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

            // TODO: pagination may be needed
            model.Results = results;

            return View(model);
        }
    }
}