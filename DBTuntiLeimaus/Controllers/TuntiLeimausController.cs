using DBTuntiLeimaus.DataAccess;
using DBTuntiLeimaus.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DBTuntiLeimaus.Controllers
{
    public class TuntiLeimausController : Controller
    {
        // GET: TuntiLeimaus
        public bool OK { get; private set; }
        [Authorize]
        // GET: Leimaus
        public ActionResult Index()
        {
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            LeimausViewModel view = new LeimausViewModel();

            ViewBag.LuokkahuoneID = new SelectList(entities.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi");
            return View();
        }
        public JsonResult GetList()
        {
            string userInId = User.Identity.GetUserId();
            //Tämä malli antaa enemmän mahdollisuuksia
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            CultureInfo fiFi = new CultureInfo("fi-FI");
            var model = (from t in entities.TuntiRaportti
                         join au in entities.AspNetUsers on t.OppilasID equals au.Id
                         where userInId == t.OppilasID
                         select new
                         {
                             t.IDleimaus,
                             t.OppilasID,
                             EmployeeName = au.FirstName + " " + au.LastName,
                             t.Sisaan,
                             t.Ulos,
                             t.LuokkahuoneID


                         }).ToList();
           

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sisaan(TuntiRaportti pro)
        {
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            bool OK = false;

            if (pro.IDleimaus == 0)

            {
                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                TuntiRaportti dbItem = new TuntiRaportti()

                {
                    IDleimaus = pro.IDleimaus,
                    LuokkahuoneID = pro.LuokkahuoneID,
                    OppilasID = User.Identity.GetUserId(),
                    Sisaan = DateTime.Now,
                };
                
                // tallennus tietokantaan
                entities.TuntiRaportti.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }

            entities.Dispose();
            return Json(OK, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Ulos(TuntiRaportti pro)
        {
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            //haetaan id:n perusteella rivi SQL tietokannasta
            string userInId = User.Identity.GetUserId();
            TuntiRaportti dbItem = (from t in entities.TuntiRaportti
                                    where t.OppilasID == userInId
                                    orderby t.IDleimaus descending
                                    select t).First();


            {

                dbItem.Ulos = DateTime.Now;

                // tallennus SQL tietokantaan
                entities.SaveChanges();
                OK = true;
                ModelState.Clear();
            }

            //entiteettiolion vapauttaminen
            entities.Dispose();

            // palautetaan 'json' muodossa
            return Json(OK, JsonRequestBehavior.AllowGet);

        }
        [Authorize(Roles = "Opettaja, Admin, SuperUser")]
        public ActionResult TuntiRaporttiOpettaja()


        {
            //tähän luotu luokka
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            try
            {
                string userInId = User.Identity.GetUserId();

                // haetaan kaikki kuluvan päivän tuntikirjaukset
                List<TuntiRaportti> time = (from t in entities.TuntiRaportti
                                            orderby t.IDleimaus descending
                                            select t).ToList();

                // ryhmitellään kirjaukset tehtävittäin ja lasketaan kestot

                List<LeimausViewModel> model = new List<LeimausViewModel>();

                foreach (TuntiRaportti tuntiRaportti in time)
                {

                    LeimausViewModel view = new LeimausViewModel();

                    view.ID = tuntiRaportti.IDleimaus;
                    view.Nimi = tuntiRaportti.AspNetUsers.LastName + " " + tuntiRaportti.AspNetUsers.FirstName;
                    view.Sisään = tuntiRaportti.Sisaan.Value;
                    view.Ulos = tuntiRaportti.Ulos.Value;
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

        [Authorize(Roles = "Oppilas, Admin")]
        public ActionResult TuntiRaporttiOppilas()


        {
            //tähän luotu luokka
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            try
            {
                string userInId = User.Identity.GetUserId();

                // haetaan kaikki kuluvan päivän tuntikirjaukset
                List<TuntiRaportti> time = (from t in entities.TuntiRaportti
                                                      where (t.OppilasID == userInId)
                                                        orderby t.IDleimaus descending
                                                        select t).ToList();

                // ryhmitellään kirjaukset tehtävittäin ja lasketaan kestot

                List<LeimausViewModel> model = new List<LeimausViewModel>();

                foreach (TuntiRaportti tuntiRaportti in time)
                {

                    LeimausViewModel view = new LeimausViewModel();

                    view.ID = tuntiRaportti.IDleimaus;
                    view.Nimi = tuntiRaportti.AspNetUsers.LastName.ToString() + " " + tuntiRaportti.AspNetUsers.FirstName.ToString();
                    view.Sisään = tuntiRaportti.Sisaan.Value;
                    view.Ulos = tuntiRaportti.Ulos.Value;
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
        public ActionResult Create()
        {

            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            ViewBag.OppilasID = new SelectList(entities.AspNetUsers, "Id", "UserName");
            ViewBag.LuokkahuoneID = new SelectList(entities.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi");
            return View();
        }

        // POST: Opettaja/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDleimaus,Sisaan,Ulos,OppilasID,LuokkahuoneID")] TuntiRaportti tuntiRaportti)
        {
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();

            if (ModelState.IsValid)
            {
                entities.TuntiRaportti.Add(tuntiRaportti);
                entities.SaveChanges();
                return RedirectToAction("Index");
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
                return RedirectToAction("TuntiRaporttiOppilas");
            }
           
            ViewBag.LuokkahuoneID = new SelectList(entities.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi", tuntiRaportti.LuokkahuoneID);
            return View(tuntiRaportti);
        }

        // GET: Opettaja/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)

         
        {
         TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
        TuntiRaportti tuntiRaportti = entities.TuntiRaportti.Find(id);
            entities.TuntiRaportti.Remove(tuntiRaportti);
            entities.SaveChanges();
            return RedirectToAction("Index");
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
    

