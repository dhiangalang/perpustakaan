using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Perpustakaan.Controllers;
using Perpustakaan.Models;

namespace Perpustakaan.Helpers
{
    public class AuthorizeUser : ActionFilterAttribute
    {
        private Role? _allowedRole;

        public AuthorizeUser()
        {
            _allowedRole = null;
        }

        public AuthorizeUser(Role role)
        {
            _allowedRole = role;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            User user = (User)filterContext.HttpContext.Session[AppConstants.SessionKey.USER_SESSION];
            if (user == null)
            {
                _RedirectToHome(filterContext);
            }
            else
            {
                if (_allowedRole != null && _allowedRole != user.Role)
                {
                    _RedirectToHome(filterContext);
                }

                // get id from param if exist
                KeyValuePair<string,object> userIdKey = filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id");
                // check if user controller
                if (typeof(UserController) == filterContext.Controller.GetType())
                {
                    // if not admin check edit, details or delete user id
                    if (user.Role != Role.Admin && !userIdKey.Equals(default(KeyValuePair<string, object>)) && (int)userIdKey.Value != user.IDUser)
                    {
                        _RedirectToHome(filterContext);
                    }
                }
            }
        }

        private void _RedirectToHome(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Session[AppConstants.SessionKey.POPUP_MESSAGE] = "Kamu kami alihkan ke halaman Beranda karena kamu tidak mempunyai akses ke halaman yang kamu tuju.";
            filterContext.Result = new RedirectResult("/");
        }
    }
}