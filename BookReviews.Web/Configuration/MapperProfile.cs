using AutoMapper;
using BookReviews.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookReviews.Web.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterVM, Impl.Models.User>()
                .ForMember(d => d.Id, src => src.Ignore())
                .ForMember(d => d.EmailAddress, src => src.MapFrom(s => s.EmailAddress))
                .ForMember(d => d.FirstName, src => src.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, src => src.MapFrom(s => s.LastName));

            CreateMap<AddReviewVM, Impl.Models.Review>()
                .ForMember(d => d.Id, src => src.Ignore())
                .ForMember(d => d.Rating, src => src.MapFrom(s => s.Rating))
                .ForMember(d => d.Notes, src => src.MapFrom(s => s.Notes))
                .ForMember(d => d.DateAdded, src => src.Ignore())
                .ForMember(d => d.DateUpdated, src => src.Ignore())
                .ForMember(d => d.Book, src => src.MapFrom(s => s.Book))
                .ForMember(d => d.Author, src => src.MapFrom(s => new Impl.Models.User() { Id = s.UserId }))
                .ForMember(d => d.Genre, src => src.MapFrom(s => new Impl.Models.Genre() { Id = s.GenreId }))
                .ForMember(d => d.BookFormat, src => src.MapFrom(s => s.BookFormat));

            CreateMap<EditReviewVM, Impl.Models.Review>()
                .ForMember(d => d.Id, src => src.MapFrom(s => s.ReviewId))
                .ForMember(d => d.Rating, src => src.MapFrom(s => s.Rating))
                .ForMember(d => d.Notes, src => src.MapFrom(s => s.Notes))
                .ForMember(d => d.DateAdded, src => src.Ignore())
                .ForMember(d => d.DateUpdated, src => src.Ignore())
                .ForMember(d => d.Book, src => src.MapFrom(s => s.Book))
                .ForMember(d => d.Author, src => src.MapFrom(s => new Impl.Models.User() { Id = s.UserId }))
                .ForMember(d => d.Genre, src => src.MapFrom(s => new Impl.Models.Genre() { Id = s.GenreId }))
                .ForMember(d => d.BookFormat, src => src.MapFrom(s => s.BookFormat));

            CreateMap<Impl.Models.Review, EditReviewVM>()
                .ForMember(d => d.ReviewId, src => src.MapFrom(s => s.Id))
                .ForMember(d => d.Rating, src => src.MapFrom(s => s.Rating))
                .ForMember(d => d.Notes, src => src.MapFrom(s => s.Notes))
                .ForMember(d => d.Book, src => src.MapFrom(s => s.Book))
                .ForMember(d => d.UserId, src => src.MapFrom(s => s.Author.Id))
                .ForMember(d => d.GenreId, src => src.MapFrom(s => s.Genre.Id))
                .ForMember(d => d.BookFormat, src => src.MapFrom(s => s.BookFormat));
        }

        //public class BookTypeConverter : ITypeConverter<AddReviewVM, Impl.Models.Book>
        //{
        //    public Impl.Models.Book Convert(AddReviewVM source, Impl.Models.Book destination, ResolutionContext context)
        //    {
        //        var book = new Impl.Models.Book()
        //        {
        //            Id = source.BookId,
        //            Title = source.B
        //        };

        //        return book;
        //    }
        //}
    }
}