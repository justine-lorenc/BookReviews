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
using Exception = System.Exception;

namespace BookReviews.Impl.Logic
{
    public class BookLogic : IBookLogic
    {
        private IMapper _mapper;
        private IExceptionLogic _exceptionLogic;
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;
        private ISearchRepository _searchRepository;

        public BookLogic(IMapper mapper, IExceptionLogic exceptionLogic, IAuthorRepository authorRepository, 
            IBookRepository bookRepository, ISearchRepository searchRepository)
        {
            _mapper = mapper;
            _exceptionLogic = exceptionLogic;
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _searchRepository = searchRepository;
        }

        public async Task<Models.Book> GetBook(long bookId)
        {
            try
            {
                if (bookId == 0 || bookId.ToString().Length < Globals.MaxLengths.Book.Id)
                    throw new Exception("Book ID is invalid");

                // check if the book exists in the database first
                Entities.Book existingBook = await _bookRepository.GetFullBook(bookId);

                if (existingBook != null)
                {
                    Models.Book bookResult = _mapper.Map<Models.Book>(existingBook);
                    return bookResult;
                }

                // if not, search for it with the book API
                List<Models.Book> searchResults = await SearchBooks(SearchCategory.Isbn, bookId.ToString());
                return searchResults.FirstOrDefault();
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                arguments.Add("BookId", bookId.ToString());

                await _exceptionLogic.LogException(ex, "Get book error", arguments);

                return null;
            }
        }

        public async Task<List<Models.Book>> GetBooks(List<long> bookIds)
        {
            try
            {
                if (bookIds == null || bookIds.Count == 0)
                    throw new Exception("Book ID list is null or empty");
                else if (bookIds.Any(x => x == 0 || x.ToString().Length < Globals.MaxLengths.Book.Id))
                    throw new Exception("Book ID list contains an invalid entry");

                // check if the books exist in the database first
                List<Entities.Book> existingBooks = await _bookRepository.GetFullBooks(bookIds);
                List<Models.Book> bookResults = _mapper.Map<List<Models.Book>>(existingBooks);
                List<long> foundBookIds = bookResults.Select(x => x.Id).ToList();

                List<long> missingBookIds = bookIds.Except(foundBookIds).ToList();

                // for any not found, call the book API
                if (missingBookIds.Any())
                {
                    foreach (long missingBookId in missingBookIds)
                    {
                        List<Models.Book> searchResults = await SearchBooks(SearchCategory.Isbn, missingBookId.ToString());
                        Models.Book missingBook = searchResults.FirstOrDefault();

                        if (missingBook == null)
                            throw new Exception($"Failed to retrieve book with ID {missingBookId}");

                        bookResults.Add(missingBook);
                    }
                }

                return bookResults;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                if (bookIds != null)
                    arguments.Add("BookIdCount", bookIds.Count.ToString());

                await _exceptionLogic.LogException(ex, "Get books error", arguments);

                return new List<Models.Book>();
            }
        }

        public async Task<List<Models.Book>> SearchBooks(SearchCategory searchCategory, string searchTerm)
        {
            try
            {
                if (!Enum.IsDefined(typeof(SearchCategory), searchCategory))
                    throw new Exception("Search category is undefined");
                else if (String.IsNullOrWhiteSpace(searchTerm))
                    throw new Exception("Search term is null or empty");
                else if (searchTerm.Length > Globals.MaxLengths.SearchTerm)
                    throw new Exception("Search term exceeds max length");

                List<Entities.BookSearchResult> searchRecords = await _searchRepository.SearchBooks(searchCategory, searchTerm);

                // exclude any books that don't have an ISBN-13 number, as that is the unique identifier
                searchRecords = searchRecords.Where(x => x.IsValid(out string errorMessage)).Select(x => x).ToList();

                List<Models.Book> books = _mapper.Map<List<Models.Book>>(searchRecords);
                
                // remove books that have invalid core details or have not yet been published
                var bookResults = new List<Models.Book>();
                foreach (var book in books)
                {
                    if (book.IsValid(out string errorMessage))
                        bookResults.Add(book);
                }

                return bookResults;
            }
            catch (Exception ex)
            {
                var arguments = new Dictionary<string, string>();
                arguments.Add("SearchCategory", searchCategory.ToString());
                arguments.Add("SearchTerm", searchTerm);

                await _exceptionLogic.LogException(ex, "Search books error", arguments);

                return new List<Models.Book>();
            }
        }

        public async Task<bool> AddBook(Models.Book book)
        {
            try
            {
                if (book == null)
                    throw new Exception("Book is null");
                else if (!book.IsValid(out string errorMessage))
                    throw new Exception(errorMessage);

                // insert the book if it doesn't already exist
                Entities.Book bookRecord = await _bookRepository.GetBook(book.Id);

                if (bookRecord != null)
                    return true;

                bookRecord = _mapper.Map<Entities.Book>(book);
                int booksInserted = await _bookRepository.InsertBook(bookRecord);

                if (booksInserted == 0)
                    throw new Exception("Failed to insert book");

                // insert any authors that don't already exist
                List<int> authorIds = await AddAuthors(book.Authors);

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
                var arguments = new Dictionary<string, string>();
                if (book != null)
                    arguments.Add("BookId", book.Id.ToString());

                await _exceptionLogic.LogException(ex, "Add book error", arguments);

                return false;
            }
        }

        private async Task<List<int>> AddAuthors(List<Models.Author> authors)
        {
            var authorIds = new List<int>();

            if (authors == null || authors.Count == 0)
                throw new Exception("Author list is null or empty");
            else if (authors.Any(x => !x.IsValid(out string errorMessage)))
                throw new Exception("Author list contains an invalid entry");

            List<string> authorNames = authors.Select(x => x.Name).ToList();
            List<Entities.Author> existingAuthors = await _authorRepository.GetAuthors(authorNames);

            foreach (var author in authors)
            {
                Entities.Author existingAuthor = existingAuthors.Where(x => 
                    x.Name.Equals(author.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (existingAuthor != null)
                {
                    authorIds.Add(existingAuthor.Id);
                    continue;
                }

                Entities.Author authorRecord = _mapper.Map<Entities.Author>(author);
                int authorId = await _authorRepository.InsertAuthor(authorRecord);

                if (authorId == 0)
                    throw new Exception($"Failed to insert author {author.Name}");
                else
                    authorIds.Add(authorId);
            }

            return authorIds;
        }
    }
}
