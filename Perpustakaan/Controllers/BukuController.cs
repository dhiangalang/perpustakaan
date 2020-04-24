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
using Perpustakaan.Helpers;

namespace Perpustakaan.Controllers
{
    [AuthorizeUser(Role.Admin)]
    public class BukuController : Controller
    {
        private PerpustakaanContext db = new PerpustakaanContext();

        // GET: Buku
        public ActionResult Index()
        {
            return View(db.Bukus.ToList());
        }

        // GET: Buku/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buku buku = db.Bukus.Find(id);
            if (buku == null)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Buku tidak ditemukan.");
                return RedirectToAction("Index");
            }
            return View(buku);
        }

        // GET: Buku/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Buku/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDBook,JudulBuku,Pengarang,JenisBuku,HargaSewa")] Buku buku)
        {
            if (ModelState.IsValid)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Gagal membuat buku dengan judul {0}.", buku.JudulBuku);

                try
                {
                    db.Bukus.Add(buku);
                    db.SaveChanges();
                    Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Berhasil membuat buku dengan judul {0}.", buku.JudulBuku);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Can do error login here. I will leave it empty for now.
                }
            }

            return View(buku);
        }

        // GET: Buku/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buku buku = db.Bukus.Find(id);
            if (buku == null)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Buku tidak ditemukan.");
                return RedirectToAction("Index");
            }
            return View(buku);
        }

        // POST: Buku/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDBook,JudulBuku,Pengarang,JenisBuku,HargaSewa")] Buku buku)
        {
            if (ModelState.IsValid)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Gagal mengedit buku dengan judul {0}.", buku.JudulBuku);

                try
                {
                    db.Entry(buku).State = EntityState.Modified;
                    db.SaveChanges();
                    Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Berhasil mengedit buku dengan judul {0}.", buku.JudulBuku);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Can do error login here. I will leave it empty for now.
                }
            }
            return View(buku);
        }

        // GET: Buku/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buku buku = db.Bukus.Find(id);
            if (buku == null)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Buku tidak ditemukan.");
                return RedirectToAction("Index");
            }
            return View(buku);
        }

        // POST: Buku/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Buku buku = db.Bukus.Find(id);
            Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Gagal menghapus buku dengan judul {0}.", buku.JudulBuku);

            if (buku == null)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Buku tidak ditemukan.");
                return RedirectToAction("Index");
            }
            try
            {
                db.Bukus.Remove(buku);
                db.SaveChanges();
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Berhasil menghapus buku dengan judul {0}.", buku.JudulBuku);
            }
            catch (Exception ex)
            {
                // Can do error login here. I will leave it empty for now.
            }

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
