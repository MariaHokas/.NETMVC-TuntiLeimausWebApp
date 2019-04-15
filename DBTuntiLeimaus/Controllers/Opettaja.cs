using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBTuntiLeimaus.DataAccess;

namespace DBTuntiLeimaus.Controllers
{
    public class Opettaja : Controller
    {
        private TuntiLeimausDBEntities db = new TuntiLeimausDBEntities();

        // GET: Opettaja
        public ActionResult Index()
        {
            var tuntiRaportti = db.TuntiRaportti.Include(t => t.AspNetUsers).Include(t => t.Luokkahuone);
            return View(tuntiRaportti.ToList());
        }

        // GET: Opettaja/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuntiRaportti tuntiRaportti = db.TuntiRaportti.Find(id);
            if (tuntiRaportti == null)
            {
                return HttpNotFound();
            }
            return View(tuntiRaportti);
        }

        // GET: Opettaja/Create
        public ActionResult Create()
        {
            ViewBag.OppilasID = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.LuokkahuoneID = new SelectList(db.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi");
            return View();
        }

        // POST: Opettaja/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDleimaus,Sisaan,Ulos,OppilasID,LuokkahuoneID")] TuntiRaportti tuntiRaportti)
        {
            if (ModelState.IsValid)
            {
                db.TuntiRaportti.Add(tuntiRaportti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OppilasID = new SelectList(db.AspNetUsers, "Id", "Email", tuntiRaportti.OppilasID);
            ViewBag.LuokkahuoneID = new SelectList(db.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi", tuntiRaportti.LuokkahuoneID);
            return View(tuntiRaportti);
        }

        // GET: Opettaja/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuntiRaportti tuntiRaportti = db.TuntiRaportti.Find(id);
            if (tuntiRaportti == null)
            {
                return HttpNotFound();
            }
            ViewBag.OppilasID = new SelectList(db.AspNetUsers, "Id", "Email", tuntiRaportti.OppilasID);
            ViewBag.LuokkahuoneID = new SelectList(db.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi", tuntiRaportti.LuokkahuoneID);
            return View(tuntiRaportti);
        }

        // POST: Opettaja/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDleimaus,Sisaan,Ulos,OppilasID,LuokkahuoneID")] TuntiRaportti tuntiRaportti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuntiRaportti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OppilasID = new SelectList(db.AspNetUsers, "Id", "Email", tuntiRaportti.OppilasID);
            ViewBag.LuokkahuoneID = new SelectList(db.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi", tuntiRaportti.LuokkahuoneID);
            return View(tuntiRaportti);
        }

        // GET: Opettaja/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuntiRaportti tuntiRaportti = db.TuntiRaportti.Find(id);
            if (tuntiRaportti == null)
            {
                return HttpNotFound();
            }
            return View(tuntiRaportti);
        }

        // POST: Opettaja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TuntiRaportti tuntiRaportti = db.TuntiRaportti.Find(id);
            db.TuntiRaportti.Remove(tuntiRaportti);
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
