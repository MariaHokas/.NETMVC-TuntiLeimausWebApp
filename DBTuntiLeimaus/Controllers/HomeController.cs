using DBTuntiLeimaus.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBTuntiLeimaus.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (isUser())
                {
                    ViewBag.displayMenu = "Yes";
                    ViewBag.Name = user.Name;
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Kirjaudu sisään";
            }
            return View();
        }

        public Boolean isUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                if (s[0].ToString() == "Opettaja")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    

public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
 
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
