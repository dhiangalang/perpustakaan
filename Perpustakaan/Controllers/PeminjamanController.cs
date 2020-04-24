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
using System.Xml;
using System.IO;

namespace Perpustakaan.Controllers
{
    [AuthorizeUser]
    public class PeminjamanController : Controller
    {
        private PerpustakaanContext db = new PerpustakaanContext();

        // GET: Peminjaman
        public ActionResult Index()
        {
            User authUser = (User)Session[AppConstants.SessionKey.USER_SESSION];
            IQueryable<Peminjaman> peminjamans = db.Peminjamans.Include(p => p.User).Include(p => p.Buku);

            if (authUser.Role == Role.Peminjam)
            {
                peminjamans = peminjamans.Where(p => p.IDUser == authUser.IDUser);
            }

            ViewBag.GrandTotal = peminjamans.Sum(p => p.TotalHarga);
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
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Data peminjaman tidak ditemukan.");
                return RedirectToAction("Index");
            }
            return View(peminjaman);
        }

        // GET: Buku/DetailsBukuJson
        [AuthorizeUser(Role.Peminjam)]
        public ActionResult DetailsBukuJson(int id)
        {
            Buku buku = db.Bukus.Find(id);

            return Json( new
            {
                Pengarang = buku.Pengarang,
                JenisBuku = buku.JenisBuku,
                HargaSewa = buku.HargaSewa
            });
        }

        // GET: Peminjaman/Create
        [AuthorizeUser(Role.Peminjam)]
        public ActionResult Create()
        {
            ViewBag.IDBook = new SelectList(db.Bukus, "IDBook", "JudulBuku");
            return View();
        }

        // POST: Peminjaman/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(Role.Peminjam)]
        public ActionResult Create([Bind(Include = "IDPeminjaman,IDUser,IDBook,TanggalMulai,TanggalSelesai")] Peminjaman peminjaman)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int jumlahHari = (int)(peminjaman.TanggalSelesai - peminjaman.TanggalMulai).TotalDays;
                    if (jumlahHari < 1)
                    {
                        Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Minimal durasi peminjaman buku adalah 1 hari.");
                    }
                    else
                    {
                        Buku buku = db.Bukus.Find(peminjaman.IDBook);
                        User authUser = (User)Session[AppConstants.SessionKey.USER_SESSION];

                        peminjaman.IDUser = authUser.IDUser;
                        peminjaman.JumlahHari = jumlahHari;
                        peminjaman.TotalHarga = _Multiply(jumlahHari, buku.HargaSewa);

                        db.Peminjamans.Add(peminjaman);
                        db.SaveChanges();
                        Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Berhasil meminjam buku.");

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Gagal meminjam buku.");
                }
            }

            ViewBag.IDBook = new SelectList(db.Bukus, "IDBook", "JudulBuku", peminjaman.IDBook);
            return View(peminjaman);
        }

        // GET: Peminjaman/Edit/5
        [AuthorizeUser(Role.Peminjam)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peminjaman peminjaman = db.Peminjamans.Find(id);
            if (peminjaman == null)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Data peminjaman tidak ditemukan.");
                return RedirectToAction("Index");
            }
            ViewBag.IDBook = new SelectList(db.Bukus, "IDBook", "JudulBuku", peminjaman.IDBook);
            return View(peminjaman);
        }

        // POST: Peminjaman/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(Role.Peminjam)]
        public ActionResult Edit([Bind(Include = "IDPeminjaman,IDUser,IDBook,TanggalMulai,TanggalSelesai")] Peminjaman peminjaman)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int jumlahHari = (int)(peminjaman.TanggalSelesai - peminjaman.TanggalMulai).TotalDays;
                    if (jumlahHari < 1)
                    {
                        Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Minimal durasi peminjaman buku adalah 1 hari.");
                    }
                    else
                    {
                        Buku buku = db.Bukus.Find(peminjaman.IDBook);

                        peminjaman.JumlahHari = jumlahHari;
                        peminjaman.TotalHarga = _Multiply(jumlahHari, buku.HargaSewa);

                        db.Entry(peminjaman).State = EntityState.Modified;
                        db.SaveChanges();
                        Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Berhasil mengubah data peminjaman buku.");

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Gagal mengubah data peminjaman buku.");
                }
            }
            ViewBag.IDBook = new SelectList(db.Bukus, "IDBook", "JudulBuku", peminjaman.IDBook);

            return View(peminjaman);
        }

        // GET: Peminjaman/Delete/5
        [AuthorizeUser(Role.Peminjam)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peminjaman peminjaman = db.Peminjamans.Find(id);
            if (peminjaman == null)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Data peminjaman tidak ditemukan.");
                return RedirectToAction("Index");
            }
            return View(peminjaman);
        }

        // POST: Peminjaman/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(Role.Peminjam)]
        public ActionResult DeleteConfirmed(int id)
        {
            Peminjaman peminjaman = db.Peminjamans.Find(id);
            if (peminjaman == null)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Data peminjaman tidak ditemukan.");
                return RedirectToAction("Index");
            }
            try
            {
                db.Peminjamans.Remove(peminjaman);
                db.SaveChanges();
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Berhasil menghapus data peminjaman.");
            }
            catch (Exception ex)
            {
                Session[AppConstants.SessionKey.POPUP_MESSAGE] = string.Format("Gagal menghapus data peminjaman.");
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

        private int _Multiply(int x, int y)
        {
            int result = 0;

            HttpWebRequest request = _CreateMultiplySOAPWebRequest();
            XmlDocument SOAPReqBody = new XmlDocument();
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body>
                <Multiply xmlns=""http://tempuri.org/"">
                    <intA>" + x + @"</intA>
                    <intB>" + y + @"</intB>
                </Multiply>
                </soap:Body>
            </soap:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }

            using (WebResponse serviceResponse = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(serviceResponse.GetResponseStream()))
                {
                    XmlDocument serviceResult = new XmlDocument();
                    serviceResult.LoadXml(rd.ReadToEnd());

                    result = Convert.ToInt32(serviceResult.DocumentElement.FirstChild.FirstChild.FirstChild.InnerText);
                }
            }

            return result;
        }

        private HttpWebRequest _CreateMultiplySOAPWebRequest()
        {
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create("http://www.dneonline.com/calculator.asmx");
            Req.Headers.Add("SOAPAction:http://tempuri.org/Multiply");
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            Req.Method = "POST";
            return Req;
        }
    }
}
