﻿using AutoMapper;
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
    public class BookController : BaseController
    {
        private IMapper _mapper;
        private IBookLogic _bookLogic;

        public BookController(IMapper mapper, IBookLogic bookLogic)
        {
            _mapper = mapper;
            _bookLogic = bookLogic;
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<ActionResult> Index(long id)
        {
            if (id == default)
                return View();

            List<Book> results = await _bookLogic.SearchBooks(SearchCategory.Isbn, id.ToString());
            Book model = results.FirstOrDefault();

            return View(model);
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

            // group books with the same title and author, then display the earliest published version
            // from each group
            results = results.GroupBy(x => new { x.Title, Author = x.Authors.FirstOrDefault() })
                .Select(x => x.OrderBy(y => y.DatePublished).First()).ToList();

            // TODO: pagination may be needed
            model.Results = results;

            return View(model);
        }
    }
}