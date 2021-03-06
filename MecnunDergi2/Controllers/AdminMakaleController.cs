﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MecnunDergi2.Models;

namespace MecnunDergi2.Controllers
{
    public class AdminMakaleController : Controller
    {
        private MecnunDergiEntities db = new MecnunDergiEntities();

        // GET: AdminMakale
        public ActionResult Index()
        {
            var makale = db.Makale.Include(m => m.Kategori);
            return View(makale.ToList());
        }

        // GET: AdminMakale/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = db.Makale.Find(id);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAdı");
            return View();
        }

        // POST: AdminMakale/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "MakaleId,Baslik,İcerik,Foto,Tarih,KategoriId,UyeId,Okunma")] Makale makale)
        {
            if (ModelState.IsValid)
            {
                makale.UyeId = Convert.ToInt32(Session["uyeid"]);
                db.Makale.Add(makale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAdı", makale.KategoriId);
            return View(makale);
        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = db.Makale.Find(id);
            if (makale == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAdı", makale.KategoriId);
            return View(makale);
        }

        // POST: AdminMakale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "MakaleId,Baslik,İcerik,Foto,Tarih,KategoriId,UyeId,Okunma")] Makale makale)
        {
            if (ModelState.IsValid)
            {
                makale.UyeId = Convert.ToInt32(Session["uyeid"]);
                db.Entry(makale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAdı", makale.KategoriId);
            return View(makale);
        }

        // GET: AdminMakale/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = db.Makale.Find(id);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: AdminMakale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Makale makale = db.Makale.Find(id);
            db.Makale.Remove(makale);
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
