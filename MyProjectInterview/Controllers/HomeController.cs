using MyProjectInterview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProjectInterview.Controllers
{
    public class HomeController : Controller
    {
        private Programming_Tast1Entities db = new Programming_Tast1Entities();
        public ActionResult Index()
        {
            return View();
        }





        public ActionResult Create()
        {
            ViewBag.t_id = new SelectList(db.Type_Login.Where(x => x.t_id == 2).ToList(), "t_id", "t_name");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,contact_number,email,password,t_id")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                var row = db.Registrations.Where(x => x.email == registration.email).SingleOrDefault();
                if (row != null)
                {
                    TempData["Same Email"] = "Email already Available !!  Please Login Here.";
                    return RedirectToAction("LoginForm");
                }
                else
                {
                    db.Registrations.Add(registration);
                    db.SaveChanges();
                    return RedirectToAction("LoginForm","Home");
                }
            }
            ViewBag.t_id = new SelectList(db.Type_Login.Where(x=>x.t_id==2).ToList(), "t_id", "t_name", registration.t_id);

         
            return View(registration);
        }











        public ActionResult LoginForm()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult LoginForm(Registration reg)
        {



            var u = db.Registrations.Where(x => x.email == reg.email && x.password == reg.password).SingleOrDefault();
            if (u != null)
            {
                var row = db.Registrations.Where(x => x.email == u.email).SingleOrDefault();

                Session["Id"] = row.t_id.ToString();
             
                Session["wwId"] = row.id.ToString();
                if (Session["Id"].Equals("1"))
                {
                    TempData["AdminPanel"] = "Thanks you for your Registration";
                    return RedirectToAction("Index", "Admin");
                }
                else if (Session["Id"].Equals("2"))
                {
                    TempData["after registration"] = "Thanks you for your Registration ";
                    return RedirectToAction("Index", "Home");
                }

               

            }
            else
            {
                TempData["AdminPanel"] = "Invalid Email Or Password";
                return RedirectToAction("LoginForm", "Regstrations");
            }

            return RedirectToAction("LoginForm");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}