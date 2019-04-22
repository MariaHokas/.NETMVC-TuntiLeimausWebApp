using DBTuntiLeimaus.DataAccess;
using DBTuntiLeimaus.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DBTuntiLeimaus.Controllers
{
    public class OpettajaRaporttiController : Controller
    {

        // GET: OpettajaRaportti
        public ActionResult Index()
        {
            return View();
        }
        //[Authorize(Roles = "Opettaja, Admin, SuperUser")]
        public ActionResult TuntiRaporttiOpettaja()


        {
            //tähän luotu luokka
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            try
            {

                // haetaan kaikki kuluvan päivän tuntikirjaukset
                List<TuntiRaportti> time = (from t in entities.TuntiRaportti
                                            orderby t.IDleimaus descending
                                            select t).ToList();

                // ryhmitellään kirjaukset tehtävittäin ja lasketaan kestot

                List<LeimausViewModel> model = new List<LeimausViewModel>();

                foreach (TuntiRaportti tuntiRaportti in time)
                {

                    LeimausViewModel view = new LeimausViewModel();

                    view.Id = tuntiRaportti.IDleimaus;
                    view.Nimi = tuntiRaportti.AspNetUsers.LastName.ToString() + " " + tuntiRaportti.AspNetUsers.FirstName.ToString();
                    view.Sisään = tuntiRaportti.Sisaan.GetValueOrDefault();
                    view.Ulos = tuntiRaportti.Ulos.GetValueOrDefault();
                    view.Luokkahuone = tuntiRaportti.Luokkahuone.LuokkahuoneenNimi.ToString();

                    model.Add(view);
                }

                return View(model);

            }
            finally
            {
                entities.Dispose();
            }
        }
        // GET: Opettaja/Create
        //[Authorize(Roles = "Opettaja, Admin, SuperUser")]
        public ActionResult Create()
        {

            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            ViewBag.OppilasID = new SelectList(entities.AspNetUsers, "Id", "UserName");
            ViewBag.LuokkahuoneID = new SelectList(entities.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi");
            return View();
        }

        // POST: Opettaja/Create
        [HttpPost]
        //[Authorize(Roles = "Opettaja, Admin, SuperUser")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDleimaus,Sisaan,Ulos,OppilasID,LuokkahuoneID")] TuntiRaportti tuntiRaportti)
        {
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();

            if (ModelState.IsValid)
            {
                entities.TuntiRaportti.Add(tuntiRaportti);
                entities.SaveChanges();
                return RedirectToAction("TuntiRaporttiOpettaja");
            }

            ViewBag.OppilasID = new SelectList(entities.AspNetUsers, "Id", "UserName", tuntiRaportti.OppilasID);
            ViewBag.LuokkahuoneID = new SelectList(entities.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi", tuntiRaportti.LuokkahuoneID);
            return View(tuntiRaportti);
        }

        // GET: Opettaja/Edit/5
        public ActionResult Edit(int? id)
        {
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuntiRaportti tuntiRaportti = entities.TuntiRaportti.Find(id);
            if (tuntiRaportti == null)
            {
                return HttpNotFound();
            }
            ViewBag.OppilasID = new SelectList(entities.AspNetUsers, "Id", "UserName", tuntiRaportti.OppilasID);
            ViewBag.LuokkahuoneID = new SelectList(entities.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi", tuntiRaportti.LuokkahuoneID);
            return View(tuntiRaportti);
        }

        // POST: Opettaja/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDleimaus,Sisaan,Ulos,OppilasID,LuokkahuoneID")] TuntiRaportti tuntiRaportti)
        {
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            if (ModelState.IsValid)
            {

                entities.Entry(tuntiRaportti).State = EntityState.Modified;
                entities.SaveChanges();
                return RedirectToAction("TuntiRaporttiOpettaja");
            }
            ViewBag.OppilasID = new SelectList(entities.AspNetUsers, "Id", "UserName", tuntiRaportti.OppilasID);
            ViewBag.LuokkahuoneID = new SelectList(entities.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi", tuntiRaportti.LuokkahuoneID);
            return View(tuntiRaportti);
        }



        // GET: Opettaja/Delete/5
        //[Authorize(Roles = "Opettaja, Admin, SuperUser")]
        public ActionResult Delete(int? id)
        {

            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuntiRaportti tuntiRaportti = entities.TuntiRaportti.Find(id);
            if (tuntiRaportti == null)
            {
                return HttpNotFound();
            }
            return View(tuntiRaportti);
        }

        // POST: Opettaja/Delete/5
        //[Authorize(Roles = "Opettaja, Admin, SuperUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)


        {
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            TuntiRaportti tuntiRaportti = entities.TuntiRaportti.Find(id);
            entities.TuntiRaportti.Remove(tuntiRaportti);
            entities.SaveChanges();
            return RedirectToAction("TuntiRaporttiOpettaja");
        }

        protected override void Dispose(bool disposing)
        {

            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            if (disposing)
            {
                entities.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}