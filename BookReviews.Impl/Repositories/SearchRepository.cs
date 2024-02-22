using BookReviews.Impl.Entities;
using BookReviews.Impl.Models.Enums;
using BookReviews.Impl.Repositories.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BookSearchResult = BookReviews.Impl.Entities.BookSearchResult;
using Exception = System.Exception;

namespace BookReviews.Impl.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private IHttpClientFactory _httpClientFactory;

        public SearchRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<BookSearchResult>> SearchBooks(SearchCategory searchCategory, string searchTerm)
        {
            var books = new List<BookSearchResult>();

            string requestUrl = ConstructQueryUrl(searchCategory, searchTerm);

            var client = _httpClientFactory.CreateClient(Globals.AppSettings.BookSearchClientName);
            var response = await client.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
                throw new System.Exception($"Search failed: {(short)response.StatusCode} {response.ReasonPhrase}");

            string content = await response.Content.ReadAsStringAsync();
            books = ParseBookSearchResults(content);

            return books;
        }

        private string ConstructQueryUrl(SearchCategory searchCategory, string searchTerm)
        {
            // for more filter options see https://developers.google.com/books/docs/v1/using#WorkingVolumes

            // API method
            string queryUrl = "books/v1/volumes?";

            // append search parameters
            string queryType = String.Empty;

            switch (searchCategory)
            {
                case SearchCategory.Title:
                    queryType = "intitle:";
                    break;
                case SearchCategory.Author:
                    queryType += "inauthor:";
                    break;
                case SearchCategory.Isbn:
                    queryType += "isbn:";
                    break;
                default:
                    throw new Exception("Search category is invalid");
            }

            queryUrl += $"q={queryType}{HttpUtility.UrlEncode(searchTerm)}";

            // only retrieve books
            queryUrl += "&printType=books";

            // written in English
            queryUrl += "&langRestrict=en";

            // max results to return
            queryUrl += "&maxResults=40";

            // authenticate the request with the API key
            queryUrl += $"&key={Globals.AppSettings.GoogleBooksApiKey}";

            return queryUrl;

        }

        private List<BookSearchResult> ParseBookSearchResults(string jsonResponse)
        {
            var results = new List<BookSearchResult>();

            JObject responseData = JObject.Parse(jsonResponse);
            List<JToken> searchResults = responseData["items"]?.Children()["volumeInfo"]?.ToList() ?? new List<JToken>();

            foreach (JToken searchResult in searchResults)
            {
                // if the book is missing any core details, skip it
                if (searchResult["title"] == null || searchResult["pageCount"] == null 
                    || searchResult["description"] == null|| searchResult["publishedDate"] == null 
                    || searchResult["industryIdentifiers"] == null || searchResult["authors"] == null)
                    continue;

                try
                {
                    BookSearchResult bookSearchResult = new BookSearchResult()
                    {
                        Title = (string)searchResult["title"],
                        SubTitle = (string)searchResult["subtitle"] ?? String.Empty,
                        PageCount = Int16.TryParse((string)searchResult["pageCount"], out short pages) ? pages : (short)0,
                        Description = (string)searchResult["description"],
                    };

                    // for older books, the publication date is typically just the year
                    string publicationDate = (string)searchResult["publishedDate"];
                    if (DateTime.TryParse(publicationDate, out DateTime publishedDate))
                        bookSearchResult.PublishedDate = publishedDate;
                    else
                        bookSearchResult.PublishedDate = new DateTime(Convert.ToInt32(publicationDate), 1, 1);

                    bookSearchResult.IndustryIdentifiers = searchResult["industryIdentifiers"].Select(x => x.ToObject<IndustryIdentifier>()).ToList();
                    bookSearchResult.ImageLinks = searchResult["imageLinks"]?.ToObject<ImageLinks>() ?? null;
                    bookSearchResult.Authors = searchResult["authors"].Select(x => (string)x).ToList();

                    results.Add(bookSearchResult);
                }
                catch
                {
                    // swallow individual failures
                }
            }

            return results;
        }
    }
}
