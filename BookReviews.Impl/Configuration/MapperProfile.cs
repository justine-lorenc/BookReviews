using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Dictionary<string, string>, string>().ConvertUsing<ArgumentStringConverter>();

            CreateMap<Entities.Author, Models.Author>();
            CreateMap<Models.Author, Entities.Author>();

            CreateMap<string, Models.Author>().ConvertUsing<AuthorTypeConverter>();

            CreateMap<Entities.Book, Models.Book>();
            CreateMap<Models.Book, Entities.Book>();

            CreateMap<Entities.BookSearchResult, Models.Book>()
                .ForMember(d => d.Id, src => src.MapFrom(s =>
                    s.IndustryIdentifiers.Where(x => String.Equals(x.Type, "ISBN_13", StringComparison.OrdinalIgnoreCase))
                    .Select(x => Convert.ToInt64(x.Identifier)).FirstOrDefault()))
                .ForMember(d => d.Title, src => src.MapFrom(s => s.Title))
                .ForMember(d => d.SubTitle, src => src.MapFrom(s => s.SubTitle))
                .ForMember(d => d.Pages, src => src.MapFrom(s => s.PageCount))
                .ForMember(d => d.DatePublished, src => src.MapFrom(s => s.PublishedDate))
                .ForMember(d => d.Description, src => src.MapFrom(s => s.Description))
                .ForMember(d => d.CoverUrl, src => src.MapFrom(s => 
                    (s.ImageLinks != null && !String.IsNullOrWhiteSpace(s.ImageLinks.Thumbnail)) ? s.ImageLinks.Thumbnail : String.Empty))
                .ForMember(d => d.Authors, src => src.MapFrom(s => s.Authors));

            CreateMap<System.Exception, Entities.Exception>()
                .ForMember(d => d.Id, src => src.Ignore())
                .ForMember(d => d.Error, src => src.MapFrom(s => s.Message))
                .ForMember(d => d.StackTrace, src => src.MapFrom(s => s.StackTrace))
                .ForMember(d => d.DateAdded, src => src.Ignore())
                .ForMember(d => d.Message, src => src.Ignore())
                .ForMember(d => d.Method, src => src.Ignore())
                .ForMember(d => d.Arguments, src => src.Ignore())
                .ForMember(d => d.UserId, src => src.Ignore());

            CreateMap<Entities.Genre, Models.Genre>();
            CreateMap<Models.Genre, Entities.Genre>();

            CreateMap<Models.NewAccount, Entities.User>()
                .ForMember(d => d.Id, src => src.Ignore())
                .ForMember(d => d.FirstName, src => src.MapFrom(d => d.FirstName))
                .ForMember(d => d.LastName, src => src.MapFrom(d => d.LastName))
                .ForMember(d => d.DateAdded, src => src.Ignore())
                .ForMember(d => d.DateUpdated, src => src.Ignore())
                .ForMember(d => d.EmailAddress, src => src.MapFrom(d => d.EmailAddress))
                .ForMember(d => d.PasswordHash, src => src.Ignore())
                .ForMember(d => d.IsActive, src => src.Ignore());

            CreateMap<Entities.Review, Models.Review>();
            CreateMap<Models.Review, Entities.Review>();

            CreateMap<Entities.User, Models.User>();
            CreateMap<Models.User, Entities.User>();
        }

        public class AuthorTypeConverter: ITypeConverter<string, Models.Author>
        {
            public Models.Author Convert(string source, Models.Author destination, ResolutionContext context)
            {
                var author = new Models.Author() { Name = source };
                return author;
            }
        }

        public class ArgumentStringConverter: ITypeConverter<Dictionary<string, string>, string>
        {
            public string Convert(Dictionary<string, string> source, string destination, ResolutionContext context)
            {
                StringBuilder builder = new StringBuilder();

                if (source != null)
                {
                    foreach (var argument in source)
                    {
                        builder.Append($"{argument.Key}: {argument.Value} | ");
                    }
                }

                string result = builder.ToString().TrimEnd(new[] { ' ', '|' });

                return result;
            }
        }
    }
}
