using DBTuntiLeimaus.DataAccess;
using DBTuntiLeimaus.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DBTuntiLeimaus.Controllers
{
    public class TuntiLeimausController : Controller
    {
        // GET: TuntiLeimaus
        public bool OK { get; private set; }      
        //[Authorize(Roles = "Oppilas")]
        // GET: Leimaus
        public ActionResult Index()
        {
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
            LeimausViewModel view = new LeimausViewModel();

            ViewBag.LuokkahuoneID = new SelectList(entities.Luokkahuone, "LuokkahuoneID", "LuokkahuoneenNimi");
            return View();
        }
        //[Authorize(Roles = "Oppilas")]
        public JsonResult GetList()
        {
            string userInId = User.Identity.GetUserId();
            //Tämä malli antaa enemmän mahdollisuuksia
            TuntiLeimausDBEntities entities = new TuntiLeimausDBEntities();
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


        [Authorize(Roles = "Oppilas")]
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
        [Authorize(Roles = "Oppilas")]
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

        //[Authorize(Roles = "Oppilas")]
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

    }
}
    

