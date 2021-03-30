using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyProjectInterview.Models;

namespace MyProjectInterview.Controllers
{
    public class AdminController : Controller
    {
        private Programming_Tast1Entities db = new Programming_Tast1Entities();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["wwId"] != null)
            {
                var registrations = db.Registrations.Include(r => r.Type_Login);
                return View(registrations.ToList());
            }
            else 
                {

                } return RedirectToAction("LoginForm", "Home");
           
        }

      
        public ActionResult Create()
        {
            ViewBag.t_id = new SelectList(db.Type_Login, "t_id", "t_name");
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
                    return RedirectToAction("Inxex");
                }
                else
                {
                    db.Registrations.Add(registration);
                db.SaveChanges();
                return RedirectToAction("LoginForm","Home");
            }
        }
            //ViewBag.t_id = new SelectList(db.Type_Login.Where(x=>x.t_id==2).ToList(), "t_id", "t_name", registration.t_id);

            ViewBag.t_id = new SelectList(db.Type_Login, "t_id", "t_name", registration.t_id);
            return View(registration);
        }

        // GET: Admin/Edit/5
       


    }
}
