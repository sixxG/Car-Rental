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
            return View(db.Admin_Tbl.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin_Tbl admin_Tbl = db.Admin_Tbl.Find(id);
            if (admin_Tbl == null)
            {
                return HttpNotFound();
            }
            return View(admin_Tbl);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
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
                var userID = User.Identity.GetUserId().ToString();
                var login = User.Identity.GetUserName();
                var role = UserManager.GetRoles(User.Identity.GetUserId());
                admin_Tbl.user_ID = userID;
                admin_Tbl.Login = login.ToString();
                admin_Tbl.Password = role.First().ToString();
                db.Admin_Tbl.Add(admin_Tbl);
                db.SaveChanges();
                return View(admin_Tbl);
            }

            return View(admin_Tbl);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin_Tbl admin_Tbl = db.Admin_Tbl.Where(admin => admin.user_ID.Equals(id)).First();
            if (admin_Tbl == null)
            {
                return HttpNotFound();
            }
            return View(admin_Tbl);
        }

        // POST: Admin/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FIO,Login,Password,user_ID")] Admin_Tbl admin_Tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin_Tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin_Tbl);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin_Tbl admin_Tbl = db.Admin_Tbl.Find(id);
            if (admin_Tbl == null)
            {
                return HttpNotFound();
            }
            return View(admin_Tbl);
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
