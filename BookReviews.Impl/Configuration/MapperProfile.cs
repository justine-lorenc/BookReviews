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
            CreateMap<Entities.Book, Models.Book>();
            CreateMap<Models.Book, Entities.Book>();

            CreateMap<Entities.Genre, Models.Genre>();
            CreateMap<Models.Genre, Entities.Genre>();

            CreateMap<Entities.Review, Models.Review>();
            CreateMap<Models.Review, Entities.Review>();
        }
    }
}
