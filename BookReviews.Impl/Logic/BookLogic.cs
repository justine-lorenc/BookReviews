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
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;
        private ISearchRepository _searchRepository;

        public BookLogic(IMapper mapper, IAuthorRepository authorRepository, IBookRepository bookRepository, ISearchRepository searchRepository)
        {
            _mapper = mapper;
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _searchRepository = searchRepository;
        }

        public async Task<Models.Book> GetBook(long id)
        {
            try
            {
                // check if the book exists in the database first
                Entities.Book existingBook = await _bookRepository.GetFullBook(id);

                if (existingBook != null)
                {
                    Models.Book bookResult = _mapper.Map<Models.Book>(existingBook);
                    return bookResult;
                }

                // if not, search for it with the book API
                List<Models.Book> searchResults = await SearchBooks(SearchCategory.Isbn, id.ToString());
                return searchResults.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // log error here
                return null;
            }
        }

        public async Task<List<Models.Book>> SearchBooks(SearchCategory searchCategory, string searchTerm)
        {
            try
            {
                List<Entities.BookSearchResult> searchRecords = await _searchRepository.SearchBooks(searchCategory, searchTerm);

                // exclude any books that don't have an ISBN-13 number, as that is the unique identifier
                searchRecords = searchRecords.Where(x => x.IndustryIdentifiers.Any(y => y.Type.Equals("ISBN_13", StringComparison.OrdinalIgnoreCase)))
                    .Select(x => x).ToList();

                List<Models.Book> books = _mapper.Map<List<Models.Book>>(searchRecords);
                
                // remove books that have invalid core details or have not yet been published
                var bookResults = new List<Models.Book>();
                foreach (var book in books)
                {
                    if (ValidateBook(book))
                        bookResults.Add(book);
                }

                return bookResults;
            }
            catch (Exception ex)
            {
                // log error here
                return new List<Models.Book>();
            }
        }

        public async Task<bool> AddBook(Models.Book book)
        {
            try
            {
                // insert the book if it doesn't already exist
                Entities.Book bookRecord = await _bookRepository.GetBook(book.Id);

                if (bookRecord != null)
                    return true;

                bookRecord = _mapper.Map<Entities.Book>(book);
                int booksInserted = await _bookRepository.InsertBook(bookRecord);

                if (booksInserted == 0)
                    throw new Exception("Failed to insert book");

                // insert authors that don't already exist
                List<int> authorIds = await AddAuthors(book.Id, book.Authors);

                if (authorIds.Count < book.Authors.Count)
                    throw new Exception("Failed to insert authors");

                // link the book and authors
                int bookAuthorsInserted = await _authorRepository.InsertBookAuthors(book.Id, authorIds);

                if (bookAuthorsInserted < book.Authors.Count)
                    throw new Exception("Failed to insert book authors");

                return true;
            }
            catch (Exception ex)
            {
                // log error here
                return false;
            }
        }

        private async Task<List<int>> AddAuthors(long bookId, List<Models.Author> authors)
        {
            var authorIds = new List<int>();

            foreach (var author in authors)
            {
                Entities.Author authorRecord = await _authorRepository.GetAuthor(author.Name);

                if (authorRecord != null)
                {
                    authorIds.Add(authorRecord.Id);
                    continue;
                }

                authorRecord = _mapper.Map<Entities.Author>(author);
                int authorId = await _authorRepository.InsertAuthor(authorRecord);

                if (authorId == 0)
                    throw new Exception("Failed to insert author");
                else
                    authorIds.Add(authorId);
            }

            return authorIds;
        }

        private bool ValidateBook(Models.Book book)
        {
            if (book.Id != default && !String.IsNullOrWhiteSpace(book.Title) && book.Pages != default
                && book.DatePublished <= DateTime.Today && book.Authors.Count > 0 && !String.IsNullOrWhiteSpace(book.Description))
                return true;
            else
                return false;
        }
    }
}
