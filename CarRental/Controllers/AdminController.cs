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
using EntityState = System.Data.Entity.EntityState;

namespace CarRental.Controllers
{
    public class AdminController : Controller
    {
        private CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Message = "Admins";
            return View(db.Admin_Tbl.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Message = "Admin Details";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin_Tbl admin_Tbl = db.Admin_Tbl.Find(id);
            if (admin_Tbl == null)
            {
                return HttpNotFound();
            }
            return View("Details",admin_Tbl);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.Message = "Create Admin";
            return View();
        }

        // POST: Admin/Create
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
        public ActionResult Create([Bind(Include = "id,FIO,Login,Password,user_ID")] Admin_Tbl admin_Tbl)
        {
            if (ModelState.IsValid)
            {
                if (User != null)
                {
                    var userID = User.Identity.GetUserId().ToString();
                    var login = User.Identity.GetUserName();
                    var role = UserManager.GetRoles(User.Identity.GetUserId());
                    admin_Tbl.user_ID = userID;
                    admin_Tbl.Login = login.ToString();
                    admin_Tbl.Password = role.First().ToString();
                }
                db.Admin_Tbl.Add(admin_Tbl);
                db.SaveChanges();
                return View("Create", admin_Tbl);
            }

            return View("Create",admin_Tbl);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string user_ID)
        {
            ViewBag.Message = "Edit Admin";
            if (user_ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin_Tbl admin_Tbl = db.Admin_Tbl.Where(admin => admin.user_ID.Equals(user_ID)).First();
            if (admin_Tbl == null)
            {
                return HttpNotFound();
            }
            return View("Edit",admin_Tbl);
        }

        // POST: Admin/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FIO,Login,Password,user_ID")] Admin_Tbl admin_Tbl)
        {
            if (User != null)
            {
                admin_Tbl.Login = User.Identity.GetUserName();
            }
            admin_Tbl.Password = "Admin";
            if (ModelState.IsValid)
            {
                db.Entry(admin_Tbl).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Admin was Edited";
                return View("Edit",admin_Tbl);
                //return RedirectToAction("Index");
            }
            ViewBag.Message = "Admin was`nt Edited";
            return View("Edit",admin_Tbl);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Message = "Delete Admin";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin_Tbl admin_Tbl = db.Admin_Tbl.Find(id);
            if (admin_Tbl == null)
            {
                return HttpNotFound();
            }
            return View("Delete",admin_Tbl);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin_Tbl admin_Tbl = db.Admin_Tbl.Find(id);
            db.Admin_Tbl.Remove(admin_Tbl);
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
