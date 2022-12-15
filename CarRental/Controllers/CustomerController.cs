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
    public class CustomerController : Controller
    {
        private CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

        // GET: Customer
        public ActionResult Index()
        {
            return View(db.Customer_Tbl.ToList());
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_Tbl customer_Tbl = db.Customer_Tbl.Find(id);
            if (customer_Tbl == null)
            {
                return HttpNotFound();
            }
            return View(customer_Tbl);
        }



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


        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FIO,BirthDate,Passport_Data,Drivers_License,Address,Login,Password,user_ID")] Customer_Tbl customer_Tbl)
        {
            if (ModelState.IsValid)
            {
                var userID = User.Identity.GetUserId().ToString();
                var login = User.Identity.GetUserName();
                var password = UserManager.GetRoles(User.Identity.GetUserId());
                customer_Tbl.user_ID = userID;
                customer_Tbl.Login = login.ToString();
                customer_Tbl.Password = password.First().ToString();
                db.Customer_Tbl.Add(customer_Tbl);
                db.SaveChanges();
                return View(customer_Tbl);
            }

            return View(customer_Tbl);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_Tbl customer_Tbl = db.Customer_Tbl.Where(customer => customer.user_ID.Equals(id)).First();
            if (customer_Tbl == null)
            {
                return HttpNotFound();
            }
            return View(customer_Tbl);
        }

        // POST: Customer/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FIO,BirthDate,Passport_Data,Drivers_License,Address,Login,Password,user_ID")] Customer_Tbl customer_Tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer_Tbl).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer_Tbl);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_Tbl customer_Tbl = db.Customer_Tbl.Find(id);
            
            List<Contract> contract = db.Contract.Where(contr => contr.id_client.Equals(customer_Tbl.user_ID)).ToList();
            ViewBag.ClientsRent = contract.Count();

            if (customer_Tbl == null)
            {
                return HttpNotFound();
            }
            return View(customer_Tbl);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer_Tbl customer_Tbl = db.Customer_Tbl.Find(id);

            var contract = db.Contract.Where(contr => contr.id_client.Equals(customer_Tbl.user_ID)).ToList();
            for(int i = 0; i < contract.Count(); i++)
            {
                db.Contract.Remove(contract[i]);
            }

            db.Customer_Tbl.Remove(customer_Tbl);
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
