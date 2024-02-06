using BookReviews.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookReviews.Web.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        [Route("")]
        [Route("home")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("forbidden")]
        public ActionResult Forbidden()
        {
            return View();
        }
    }
}