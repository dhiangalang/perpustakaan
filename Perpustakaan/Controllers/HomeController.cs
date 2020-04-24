using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Perpustakaan.Models;
using Perpustakaan.Helpers;

namespace Perpustakaan.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            User authUser = (User)Session[AppConstants.SessionKey.USER_SESSION];
            if (authUser != null)
            {
                ViewBag.Message = string.Format("Kamu login sebagai {0}. Role kamu adalah {1}.", authUser.NamaUser, authUser.Role);
            }
            else
            {
                ViewBag.Message = "Silakan login untuk menggunakan fitur-fitur dalam applikasi Perpustakaan Sederhana.";
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "My contact page.";

            return View();
        }
    }
}