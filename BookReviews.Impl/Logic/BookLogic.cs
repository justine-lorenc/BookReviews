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
    public class BookLogic : IBookLogic
    {
        private IMapper _mapper;
        private IBookRepository _bookRepository;

        public BookLogic(IMapper mapper, IBookRepository bookRepository)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
        }
    }
}
