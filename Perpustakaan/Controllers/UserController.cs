using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Perpustakaan.DAL;
using Perpustakaan.Models;
using Perpustakaan.Helpers;

namespace Perpustakaan.Controllers
{
    public class UserController : Controller
    {
        private PerpustakaanContext db = new PerpustakaanContext();

        // GET: User
        [AuthorizeUser]
        public ActionResult Index()
        {
            User authUser = (User)Session[AppConstants.SessionKey.USER_SESSION];
            IQueryable<User> users = db.Users;

            if (authUser != null && authUser.Role == Role.Peminjam)
            {
                users = users.Where(u => u.Role == Role.Peminjam);
            }

            return View(users.ToList());
        }

        public ActionResult Logout()
        {
            Session.Remove(AppConstants.SessionKey.USER_SESSION);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Password,NamaUser")] LoginViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users
                    .Where(u => u.NamaUser == loginUser.NamaUser)
                    .FirstOrDefault();

                if (user != null && Crypto.VerifyHashedPassword(user.Password, loginUser.Password))
                {
                    Session[AppConstants.SessionKey.USER_SESSION] = user;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(loginUser);
        }

        // GET: User/Details/5
        [AuthorizeUser]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        [AuthorizeUser(Role.Admin)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(Role.Admin)]
        public ActionResult Create([Bind(Include = "IDUser,Password,NamaUser,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                if (!((bool)ViewData[AppConstants.ViewBagKey.FORBIDEN]))
                {
                    user.Password = Crypto.HashPassword(user.Password);

                    db.Users.Add(user);
                    db.SaveChanges();
                    Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Berhasil membuat user {0}.", user.NamaUser);

                    return RedirectToAction("Index");
                }
                else
                {
                    Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Gagal membuat user {0}.", user.NamaUser);
                }
            }

            user.Password = "";
            return View(user);
        }

        // GET: User/Edit/5
        [AuthorizeUser]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Password = string.Empty;

            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public ActionResult Edit([Bind(Include = "IDUser,Password,NamaUser,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                if (!((bool)ViewData[AppConstants.ViewBagKey.FORBIDEN]))
                {
                    //prevent user with role peminjam being naughty
                    User authUser = (User)Session[AppConstants.SessionKey.USER_SESSION];
                    if (authUser.Role == Role.Peminjam)
                    {
                        user.Role = Role.Peminjam;
                    }

                    user.Password = Crypto.HashPassword(user.Password);

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Berhasil mengubah user {0}.", user.NamaUser);

                    return RedirectToAction("Index");
                }
                else
                {
                    Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Gagal mengubah user {0}.", user.NamaUser);
                }
            }

            user.Password = "";
            return View(user);
        }

        // GET: User/Delete/5
        [AuthorizeUser(Role.Admin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(Role.Admin)]
        public ActionResult DeleteConfirmed(int id)
        {
            User authUser = (User)Session[AppConstants.SessionKey.USER_SESSION];
            User user = db.Users.Find(id);

            if (user == null)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("User tidak ditemukan.");
                return RedirectToAction("Index");
            }
            if (id == authUser.IDUser)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Tidak bisa menghapus diri sendiri.");
                return RedirectToAction("Index");
            }
            if ((bool)ViewData[AppConstants.ViewBagKey.FORBIDEN])
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Gagal menghapus user {0}.", user.NamaUser);
                return RedirectToAction("Index");
            }

            db.Users.Remove(user);
            db.SaveChanges();

            Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Berhasil menghapus user {0}.", user.NamaUser);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
