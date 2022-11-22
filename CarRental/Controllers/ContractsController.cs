using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using CarRental.Models;
using Microsoft.AspNet.Identity;

namespace CarRental.Controllers
{
    public class ContractsController : Controller
    {
        private CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

        // GET: User Contracts
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId().ToString();
            var Contracts = db.Contract.Where(contract => contract.id_client.Equals(userID));
            int lenght = Contracts.Count();
            return View(Contracts.ToList());
        }

        // GET: All Contracts
        public ActionResult IndexAll()
        {
            return View(db.Contract.ToList());
        }

        // GET: Contracts by Condition
        public ActionResult IndexByCondition(string condition)
        {
            var contracts = new List<Contract>();
            switch(condition)
            {
                case "All":
                    {
                        contracts = db.Contract.ToList();
                        break;
                    }
                case "Не подтверждён":
                    {
                        contracts = db.Contract.Where(contract => contract.Condition.Equals(condition)).ToList();
                        break;
                    }
                case "Подтверждён":
                    {
                        contracts = db.Contract.Where(contract => contract.Condition.Equals(condition)).ToList();
                        break;
                    }
                case "Действует":
                    {
                        contracts = db.Contract.Where(contract => contract.Condition.Equals(condition)).ToList();
                        break;
                    }
                case "Ожидает оплаты штрафа":
                    {
                        contracts = db.Contract.Where(contract => contract.Condition.Equals(condition)).ToList();
                        break;
                    }
                case "Завершён":
                    {
                        contracts = db.Contract.Where(contract => contract.Condition.Equals(condition)).ToList();
                        break;
                    }
                case "Отменён":
                    {
                        contracts = db.Contract.Where(contract => contract.Condition.Equals(condition)).ToList();
                        break;
                    }
            }
            return View(contracts);
        }


        // GET: Contracts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contract.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: Contracts/Create
        public ActionResult Create(int id)
        {
            var car = db.Car_Tbl.Where(auto => auto.id == id).FirstOrDefault();
            var ID = car.id;
            var WIN = car.WIN_Number;
            var Model = car.Model;
            var Brand = car.Brand;
            var TypeBody = car.Type_Body;
            var Year = car.Year_Release;
            var Millage = car.Mileage;
            var Type_Transmition = car.Type_Transmission;
            var Type_Drive = car.Type_Drive;
            var Power = car.Power;
            var Price = car.Price_Per_Day;
            var Image = car.Image;

            ViewBag.ID = ID;
            ViewBag.WIN = WIN;
            ViewBag.Model = Model;
            ViewBag.Brand = Brand;
            ViewBag.TypeBody = TypeBody;
            ViewBag.Year = Year;
            ViewBag.Millage = Millage;
            ViewBag.Type_Transmition = Type_Transmition;
            ViewBag.Type_Drive = Type_Drive;
            ViewBag.Power = Power;
            ViewBag.Price = Price;
            ViewBag.Image = Image;
            return View();
        }

        // POST: Contracts/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,FIO_Customer,FIO_Manager,Car_Brand,Car_Model,Car_WIN_Number,Additional_Options,Date_Start,Date_End,Price,Condition,Notes")] Contract contract, int id)
        {
            var car = db.Car_Tbl.Where(auto => auto.id == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                string strCurrentUserId = User.Identity.GetUserId().ToString();
                contract.id_client = strCurrentUserId;
                contract.Car_Brand = car.Brand;
                contract.Car_Model = car.Model;
                contract.Car_WIN_Number = car.WIN_Number;
                contract.Condition = "Не подтверждён";
                db.Contract.Add(contract);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contract);
        }

        // GET: Contracts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contract.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FIO_Customer,FIO_Manager,Car_Brand,Car_Model,Car_WIN_Number,Additional_Options,Date_Start,Date_End,Price,Condition,Notes")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contract).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contract.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contract contract = db.Contract.Find(id);
            db.Contract.Remove(contract);
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
