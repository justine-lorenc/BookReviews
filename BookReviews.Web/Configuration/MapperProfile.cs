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
        }
    }
}