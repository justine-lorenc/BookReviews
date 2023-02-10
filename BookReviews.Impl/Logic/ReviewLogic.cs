using AutoMapper;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Impl.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Impl.Logic
{
    public class ReviewLogic : IReviewLogic
    {
        private IMapper _mapper;
        private IReviewRepository _reviewRepository;

        public ReviewLogic(IMapper mapper, IReviewRepository reviewRepository)
        {
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }
    }
}
