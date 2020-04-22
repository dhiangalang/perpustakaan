using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Perpustakaan.DAL;
using Perpustakaan.Models;

namespace Perpustakaan.Controllers
{
    public class PeminjamanController : Controller
    {
        private PerpustakaanContext db = new PerpustakaanContext();

        // GET: Peminjaman
        public ActionResult Index()
        {
            var peminjamans = db.Peminjamans.Include(p => p.User);
            return View(peminjamans.ToList());
        }

        // GET: Peminjaman/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peminjaman peminjaman = db.Peminjamans.Find(id);
            if (peminjaman == null)
            {
                return HttpNotFound();
            }
            return View(peminjaman);
        }

        // GET: Peminjaman/Create
        public ActionResult Create()
        {
            ViewBag.IDUser = new SelectList(db.Users, "IDUser", "Password");
            return View();
        }

        // POST: Peminjaman/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDPeminjaman,IDUser,IDBuku,TanggalMulai,TanggalSelesai")] Peminjaman peminjaman)
        {
            if (ModelState.IsValid)
            {
                db.Peminjamans.Add(peminjaman);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUser = new SelectList(db.Users, "IDUser", "Password", peminjaman.IDUser);
            return View(peminjaman);
        }

        // GET: Peminjaman/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peminjaman peminjaman = db.Peminjamans.Find(id);
            if (peminjaman == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUser = new SelectList(db.Users, "IDUser", "Password", peminjaman.IDUser);
            return View(peminjaman);
        }

        // POST: Peminjaman/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDPeminjaman,IDUser,IDBuku,TanggalMulai,TanggalSelesai")] Peminjaman peminjaman)
        {
            if (ModelState.IsValid)
            {
                db.Entry(peminjaman).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUser = new SelectList(db.Users, "IDUser", "Password", peminjaman.IDUser);
            return View(peminjaman);
        }

        // GET: Peminjaman/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peminjaman peminjaman = db.Peminjamans.Find(id);
            if (peminjaman == null)
            {
                return HttpNotFound();
            }
            return View(peminjaman);
        }

        // POST: Peminjaman/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Peminjaman peminjaman = db.Peminjamans.Find(id);
            db.Peminjamans.Remove(peminjaman);
            db.SaveChanges();
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
