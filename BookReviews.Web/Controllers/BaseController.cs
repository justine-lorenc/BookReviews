using BookReviews.Web.Models.Enums;
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
        protected void SetResultMessage(MessageType messageType, string message)
        {
            switch (messageType)
            {
                case MessageType.Success:
                    TempData["SuccessMessage"] = message;
                    break;
                case MessageType.Warning:
                    TempData["WarningMessage"] = message;
                    break;
                case MessageType.Error:
                    TempData["ErrorMessage"] = message;
                    break;
            }
        }
    }
}