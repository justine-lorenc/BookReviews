using AutoMapper;
using BookReviews.Impl.Entities;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Impl.Models.Enums;
using BookReviews.Impl.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Logic
{
    public class BookLogic : IBookLogic
    {
        private IMapper _mapper;
        private IBookRepository _bookRepository;

        public BookLogic(IMapper mapper, IBookRepository bookRepository)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
        }

        public async Task<List<Models.Book>> SearchBooks(SearchCategory searchCategory, string searchTerm)
        {
            try
            {
                var records = new List<Entities.BookSearchResult>();

                records = await _bookRepository.SearchBooks(searchCategory, searchTerm);

                // exclude any books that don't have an ISBN-13 number, as that is the unique identifier
                records = records.Where(x => x.IndustryIdentifiers.Any(y => y.Type.Equals("ISBN_13", StringComparison.OrdinalIgnoreCase)))
                    .Select(x => x).ToList();

                List<Models.Book> books = _mapper.Map<List<Models.Book>>(records);
                
                // remove books that have invalid core details or have not yet been published
                var results = new List<Models.Book>();
                foreach (var book in books)
                {
                    if (book.Id != default && !String.IsNullOrWhiteSpace(book.Title) && book.Pages != default
                        && book.DatePublished <= DateTime.Today && book.Authors.Count > 0 && !String.IsNullOrWhiteSpace(book.Description))
                        results.Add(book);
                }

                return results;
            }
            catch (Exception ex)
            {
                // log error here
                return new List<Models.Book>();
            }
        }
    }
}
