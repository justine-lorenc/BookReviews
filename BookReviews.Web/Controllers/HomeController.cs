using BookReviews.Impl.Logic.Interfaces;
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
        public HomeController(IExceptionLogic exceptionLogic) : base(exceptionLogic)
        {
        }

        [HttpGet]
        [Route("")]
        [Route("home")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("error")]
        public ActionResult Error(Guid? exceptionId = null)
        {
            if (exceptionId.HasValue)
                ViewBag.ErrorId = exceptionId.Value;

            return View("~/Views/Shared/Errors/Error.cshtml");
        }

        [HttpGet]
        [Route("forbidden")]
        public ActionResult Forbidden()
        {
            return View("~/Views/Shared/Errors/Forbidden.cshtml");
        }
    }
}