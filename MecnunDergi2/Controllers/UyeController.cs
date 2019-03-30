using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MecnunDergi2.Models;

namespace MecnunDergi2.Controllers
{
    public class UyeController : Controller
    {
        private MecnunDergiEntities db = new MecnunDergiEntities();

        // GET: Uye
        public ActionResult Index()
        {
            var uye = db.Uye.Include(u => u.Yetki);
            return View(uye.ToList());
        }
        public ActionResult UyeguncellemeIndex(int id)

        {
            var uye = db.Uye.Where(u => u.UyeId == id).SingleOrDefault();
            if (uye==null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        // GET: Uye/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uye.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        public ActionResult Logın()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Logın(Uye uye)
        {
            var logın = db.Uye.Where(u => u.KullanıcıAdı == uye.KullanıcıAdı).SingleOrDefault();
            if (logın.KullanıcıAdı == uye.KullanıcıAdı && logın.Sifre == uye.Sifre)
            {
                Session["uyeid"] = logın.UyeId;
                Session["kullaniciadi"] = logın.AdSoyad;
                Session["yetkiid"] = logın.YetkiId;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Uyari = "Kullanıcı Adı veya Şifre Hatalı";
                return View();
            }

        }
        public ActionResult Logout()
        {
            Session["uyeid"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        // GET: Uye/Create
        public ActionResult Create()
        {
            // ViewBag.YetkiId = new SelectList(db.Yetki, "YetkiId", "Yetki1");
            return View();
        }

        // POST: Uye/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "UyeId,AdSoyad,KullanıcıAdı,Email,Sifre,Foto,YetkiId")]*/ Uye uye, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/İmages/Profilfoto/" + newfoto);
                    uye.Foto = "/İmages/Profilfoto/" + newfoto;
                    uye.YetkiId = 2;
                    db.Uye.Add(uye);
                    db.SaveChanges();
                    Session["uyeid"] = uye.UyeId;
                    Session["kullaniciadi"] = uye.AdSoyad;
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    ModelState.AddModelError("Foto", "Fotoğraf Seçiniz");
                }
            }

            //ViewBag.YetkiId = new SelectList(db.Yetki, "YetkiId", "Yetki1", uye.YetkiId);
            return View(uye);
        }

        // GET: Uye/Edit/5
        public ActionResult Edit(int? id)
        {
            var uye = db.Uye.Where(u => u.UyeId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["uyeid"]) != uye.UyeId)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Uye uye, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                var uyes = db.Uye.Where(u => u.UyeId == id).SingleOrDefault();
                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/İmages/Profilfoto/" + newfoto);
                    uyes.Foto = "/İmages/Profilfoto/" + newfoto;
                }
                uyes.AdSoyad = uye.AdSoyad;
                uyes.KullanıcıAdı = uye.KullanıcıAdı;
                uyes.Sifre = uye.Sifre;
                uyes.Email = uye.Email;
                db.SaveChanges();
                Session["kullaniciadi"] = uyes.AdSoyad;
                return RedirectToAction("Index", "Home", new { id = uyes.UyeId });


            }
            return View(uye);
        }
        // GET: Uye/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uye.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        // POST: Uye/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Uye uye = db.Uye.Find(id);
            db.Uye.Remove(uye);
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
