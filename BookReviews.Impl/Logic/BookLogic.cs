using AutoMapper;
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
                var bookRecords = new List<Entities.BookSearchResult>();

                bookRecords = await _bookRepository.SearchBooks(searchCategory, searchTerm);

                // TODO: consolidate books with same title and author, then select
                // the earliest publication date for display
                List<Models.Book> books = _mapper.Map<List<Models.Book>>(bookRecords);
                return books;
            }
            catch (Exception ex)
            {
                // log error here
                return new List<Models.Book>();
            }
        }
    }
}
