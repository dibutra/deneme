using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MecnunDergi2.Models;

namespace MecnunDergi2.Controllers
{
    public class HomeController : Controller
    {
        MecnunDergiEntities db = new MecnunDergiEntities();
        // GET: Home
        public ActionResult Index(int? id, string query)
        {
            ViewBag.kategori = db.Kategori.ToList();

            if (!string.IsNullOrEmpty(query))
            {
                //ViewBag.Title = query + " İçin Arama Sonuçları";
                return View(db.Makale.Where(x => x.İcerik.Contains(query)).ToList());
            }

            if (id == null)
            {
                //ViewBag.Title = "Tüm Yazılar";
                return View(db.Makale.ToList().Take(5));
            }
            else
            {
                //ViewBag.Title = db.Category.Find(id).Name + "  Yazıları";
                return View(db.Makale.Where(x => x.KategoriId == id).ToList());
            }
        }
        public ActionResult MakaleDetay(int id)
        {
            ViewBag.kategori = db.Makale.ToList();
            return View(db.Makale.Find(id));
        }
        public ActionResult Hakkımızda()
        {
            return View();
        }
        public ActionResult İletisim()
        {
            return View();
        }
        public ActionResult KategoriPartial()
        {
            return View(db.Kategori.ToList());
        }
    }
}