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
using Microsoft.AspNet.Identity.EntityFramework;

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
        public ActionResult Create([Bind(Include = "Id,FIO,BirthDate,Passport_Data,Drivers_License,Address")] Customer_Tbl customer_Tbl)
        {
            if (ModelState.IsValid)
            {
                db.Customer_Tbl.Add(customer_Tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer_Tbl);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Customer/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FIO,BirthDate,Passport_Data,Drivers_License,Address")] Customer_Tbl customer_Tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer_Tbl).State = EntityState.Modified;
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
