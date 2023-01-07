using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarRental.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Controllers
{
    public class ManagerController : Controller
    {
        private CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

        // GET: Manager
        public ActionResult Index()
        {
            ViewBag.Message = "Managers";
            return View(db.Manager_Tbl.ToList());
        }

        // GET: Manager/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Message = "Manager Details";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager_Tbl manager_Tbl = db.Manager_Tbl.Find(id);
            if (manager_Tbl == null)
            {
                return HttpNotFound();
            }
            return View("Details",manager_Tbl);
        }

        // GET: Manager/Create
        public ActionResult Create()
        {
            ViewBag.Message = "Create Manager";
            return View();
        }

        // POST: Manager/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,FIO,Login,Password,user_ID")] Manager_Tbl manager_Tbl)
        {
            if (ModelState.IsValid)
            {
                if (User != null)
                {
                    var userID = User.Identity.GetUserId().ToString();
                    var login = User.Identity.GetUserName();
                    var role = UserManager.GetRoles(User.Identity.GetUserId());
                    manager_Tbl.user_ID = userID;
                    manager_Tbl.Login = login.ToString();
                    manager_Tbl.Password = role.First().ToString();
                }

                db.Manager_Tbl.Add(manager_Tbl);
                db.SaveChanges();
                return View("Create",manager_Tbl);
            }

            return View("Create",manager_Tbl);
        }

        // GET: Manager/Edit/5
        public ActionResult Edit(string user_ID)
        {
            ViewBag.Message = "Edit Manager";
            if (user_ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager_Tbl manager_Tbl = db.Manager_Tbl.Where(manager => manager.user_ID.Equals(user_ID)).First();
            if (manager_Tbl == null)
            {
                return HttpNotFound();
            }
            return View("Edit",manager_Tbl);
        }

        // POST: Manager/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FIO,Login,Password,user_ID")] Manager_Tbl manager_Tbl)
        {
            manager_Tbl.Password = "Менеджер";
            if (User != null)
            {
                manager_Tbl.Login = User.Identity.GetUserName();
            }

            if (ModelState.IsValid)
            {
                db.Entry(manager_Tbl).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return View("Edit",manager_Tbl);
                //return RedirectToAction("Index");
            }
            return View("Edit",manager_Tbl);
        }

        // GET: Manager/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Message = "Delete Manager";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager_Tbl manager_Tbl = db.Manager_Tbl.Find(id);
            if (manager_Tbl == null)
            {
                return HttpNotFound();
            }
            return View("Delete",manager_Tbl);
        }

        // POST: Manager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manager_Tbl manager_Tbl = db.Manager_Tbl.Find(id);
            db.Manager_Tbl.Remove(manager_Tbl);
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
