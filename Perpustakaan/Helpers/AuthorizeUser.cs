﻿using System;
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
                throw new HttpException(403, "Maaf, anda tidak punya akses ke fitur yang ada di halaman ini.");
            }
            else
            {
                if (_allowedRole != null && _allowedRole != user.Role)
                {
                    throw new HttpException(403, "Maaf, anda tidak punya akses ke fitur yang ada di halaman ini.");
                }

                // get id from param if exist
                KeyValuePair<string,object> userIdKey = filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id");
                // check if user controller
                if (typeof(UserController) == filterContext.Controller.GetType())
                {
                    // if not admin check edit, details or delete user id
                    if (user.Role != Models.Role.Admin && !userIdKey.Equals(default(KeyValuePair<string, object>)) && (int)userIdKey.Value != user.IDUser)
                    {
                        throw new HttpException(403, "Maaf, anda tidak punya akses ke fitur yang ada di halaman ini.");
                    }
                }
            }
        }
    }
}