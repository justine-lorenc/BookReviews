﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Role = BookReviews.Impl.Models.Enums.Role;

namespace BookReviews.Web.Models
{
    public class CurrentUser
    {
        public static int Id 
        {
            get
            {
                ClaimsPrincipal user = HttpContext.Current?.User as ClaimsPrincipal;

                if (user == null || user.Claims.Count() == 0)
                    return 0;

                string userId = user.Claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

                return Int32.Parse(userId);
            }
        }

        public static bool IsLoggedIn()
        {
            bool isAuthenticated = HttpContext.Current?.User?.Identity?.IsAuthenticated ?? false;

            return isAuthenticated;
        }

        public static bool HasAbility(Role requiredRole)
        {
            try
            {
                ClaimsPrincipal user = HttpContext.Current.User as ClaimsPrincipal;
                if (user == null || user.Claims.Count() == 0)
                    return false;

                List<Role> userRoles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => (Role)Enum.Parse(typeof(Role), x.Value)).ToList();

                if (userRoles.Contains(Role.Admin) || userRoles.Contains(requiredRole))
                    return true;
            }
            catch
            {
                // swallow
            }

            return false;
        }
    }
}