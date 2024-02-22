using BookReviews.Impl.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookReviews.Impl.Models;
using System.Web.Mvc;
using BookReviews.Web.Models.Enums;

namespace BookReviews.Web.Utilities
{
    public static class ViewExtensions
    {
        public static string GetTitleLink(string title, long bookId)
        {
            return $"<a href=\"/books/{bookId}\" class=\"book-title\">{title}</a>";
        }

        public static string GetAuthorLinks(List<Author> authors, int? listMax = null)
        {
            string authorLinks = String.Empty;
            int listLimit = listMax.HasValue ? listMax.Value : authors.Count;

            if (authors != null && authors.Count > 0)
            {
                if (authors.Count > 1)
                {
                    for (int i = 0; i < listLimit; i++)
                    {
                        string authorName = authors[i].Name;
                        authorLinks += $"{FormatAuthorLink(authorName)}, ";
                    }

                    authorLinks = authorLinks.TrimEnd(new char[] { ',', ' ' });

                    if (listMax < authors.Count)
                        authorLinks += $" (+{authors.Count - listLimit})";
                }
                else
                {
                    string authorName = authors.Select(x => x.Name).First();
                    authorLinks = FormatAuthorLink(authorName);                
                }
            }

            return authorLinks;
        }

        public static string GetCoverImage(string coverUrl, ImageSize imageSize = ImageSize.Small)
        {
            string coverImage = String.Empty;
            string imageUrl = String.Empty;
            int imageWidth = 60;
            int imageHeight = 120;

            switch (imageSize)
            {
                case ImageSize.Small:
                    imageWidth = 60;
                    imageHeight = 96;
                    break;
                case ImageSize.Medium:
                    imageWidth = 100;
                    imageHeight = 160;
                    break;
                case ImageSize.Large:
                    imageWidth = 125;
                    imageHeight = 200;
                    break;
            }

            if (String.IsNullOrWhiteSpace(coverUrl))
                imageUrl = "/Content/Images/GenericCover.png";
            else
                imageUrl = coverUrl.Replace("&edge=curl", "").Replace("http:", "https:");

            coverImage = $"<img src=\"{imageUrl}\" width=\"{imageWidth}\" height=\"{imageHeight}\" alt=\"Book cover\" />";

            return coverImage;
        }

        public static string TruncateDescription(string description, int maxLength)
        {
            if (!String.IsNullOrWhiteSpace(description) && description.Length > maxLength)
            {
                description = description.Substring(0, maxLength)
                    .TrimEnd(new char[] { ' ', ',', '-', ':', ';', '.', '!', '?', '"', '\'', '(', ')' });

                description += "...";
            }

            return description;
        }

        public static Dictionary<int, string> GetSortByDropDown()
        {
            var dictionary = new Dictionary<int, string>();

            foreach (SortBy value in EnumExtensions.GetValuesInOrder(typeof(SortBy)))
            {
                dictionary.Add((short)value, value.GetDisplayName());
            }

            return dictionary;
        }

        private static string FormatAuthorLink(string authorName)
        {
            return $"<a href=\"/books/search?searchCategory={(short)SearchCategory.Author}&searchTerm={authorName}\">{authorName}</a>";
        }
    }
}