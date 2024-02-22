using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Web.Models;
using BookReviews.Web.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookReviews.Web.Controllers
{
    [Authorize]
    [RequireHttps]
    public class BaseController : Controller
    {
        IExceptionLogic _exceptionLogic;

        public BaseController(IExceptionLogic exceptionLogic)
        {
            _exceptionLogic = exceptionLogic;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            Guid? exceptionId = null;
            int currentUserId = 0;
            string requestUrl = String.Empty;
            string referrerUrl = String.Empty;

            try
            {
                currentUserId = CurrentUser.Id;

                var request = filterContext?.HttpContext?.Request;
                if (request != null)
                {
                    requestUrl = request.Url?.ToString();
                    referrerUrl = request.UrlReferrer?.ToString();
                }
            }
            catch { }

            try
            {
                var arguments = new Dictionary<string, string>();
                arguments.Add("RequestUrl", requestUrl);
                arguments.Add("ReferrerUrl", referrerUrl);

                exceptionId = Task.Run(async () => await
                    _exceptionLogic.LogException(filterContext.Exception, "Unhandled exception", arguments, currentUserId)).Result;
            }
            catch { }

            if (filterContext?.HttpContext?.Request?.IsAjaxRequest() == true)
            {
                filterContext.Result = Json(new { success = false });
            }
            else
            {
                filterContext.Result = RedirectToAction("Error", "Home", new { exceptionId = exceptionId });
            }
        }

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