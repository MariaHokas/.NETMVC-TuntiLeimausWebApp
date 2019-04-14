using DBTuntiLeimaus.DataAccess;
using DBTuntiLeimaus.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
                             t.Ulos

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

        public ActionResult Oppilas(string userInId)
           
        {
            List<LeimausViewModel> model = new List<LeimausViewModel>();
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            userInId = User.Identity.GetUserId();
            try
            {

                List<TuntiRaportti> times = entities.TuntiRaportti.ToList();
                // muodostetaan näkymämalli tietokannan rivien pohjalta

                foreach (TuntiRaportti time in times)
                {

                    LeimausViewModel view = new LeimausViewModel();

                    view.ID = time.IDleimaus;
                    view.Nimi = time.AspNetUsers.LastName + " " + time.AspNetUsers.FirstName;
                    view.Sisaan = time.Sisaan.Value;
                    view.Ulos = time.Ulos.Value;

                    model.Add(view);
                }

            }
            finally
            {
                entities.Dispose();
            }

            return View(model);
        }

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
                                                      select t).ToList();

                // ryhmitellään kirjaukset tehtävittäin ja lasketaan kestot

                List<LeimausViewModel> model = new List<LeimausViewModel>();

                foreach (TuntiRaportti tuntiRaportti in time)
                {

                    LeimausViewModel view = new LeimausViewModel();

                    view.ID = tuntiRaportti.IDleimaus;
                    view.Nimi = tuntiRaportti.AspNetUsers.LastName + " " + tuntiRaportti.AspNetUsers.FirstName;
                    view.Sisaan = tuntiRaportti.Sisaan.Value;
                    view.Ulos = tuntiRaportti.Ulos.Value;

                    model.Add(view);
                }

                return View(model);

            }
            finally
            {
                entities.Dispose();
            }


        }
    }
    }
    

