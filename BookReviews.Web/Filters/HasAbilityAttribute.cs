using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Role = BookReviews.Impl.Models.Enums.Role;

namespace BookReviews.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HasAbilityAttribute : AuthorizeAttribute
    {
        private Role _requiredRole;

        public HasAbilityAttribute(Role requiredRole)
        {
            _requiredRole = requiredRole;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext))
                return false;

            try
            {
                ClaimsPrincipal user = httpContext.User as ClaimsPrincipal;
                List<Role> userRoles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => (Role)Enum.Parse(typeof(Role), x.Value)).ToList();

                if (userRoles.Contains(Role.Admin) || userRoles.Contains(_requiredRole))
                    return true;
            }
            catch
            {
                // swallow the exception
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { action = "Forbidden", controller = "Home" }));
        }
    }
}