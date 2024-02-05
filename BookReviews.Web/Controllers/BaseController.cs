using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookReviews.Web.Controllers
{
    [Authorize]
    [RequireHttps]
    public class BaseController : Controller
    {
    }
}