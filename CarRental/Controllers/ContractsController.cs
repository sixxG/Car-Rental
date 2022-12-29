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
using log4net;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace CarRental.Controllers
{
    public class ContractsController : Controller
    {

        private CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

        public ActionResult FindContract(DateTime? DateStart, DateTime? DateEnd, string ListBrand, string ListClients)
        {

            var cars = db.Car_Tbl.ToList();
            List<String> carsBrand = new List<String>();

            for (int i = 0; i < cars.Count; i++)
            {
                carsBrand.Add(cars[i].Brand.ToString());
            }

            carsBrand = carsBrand.Distinct().ToList();

            List<SelectListItem> ListBrand1 = new List<SelectListItem>();

            for (int i = 0; i < carsBrand.Count; i++)
            {
                ListBrand1.Add(new SelectListItem { Text = carsBrand[i].ToString(), Value = carsBrand[i].ToString() });
            }

            var clients = db.Customer_Tbl.ToList();
            List<String> clientsFIO = new List<String>();


            for (int i = 0; i < clients.Count; i++)
            {
                clientsFIO.Add(clients[i].FIO.ToString());
            }

            clientsFIO = clientsFIO.Distinct().ToList();

            List<SelectListItem> ListClients2 = new List<SelectListItem>();

            for (int i = 0; i < clientsFIO.Count; i++)
            {
                ListClients2.Add(new SelectListItem { Text = clientsFIO[i].ToString(), Value = clientsFIO[i].ToString() });
            }

            ViewBag.ListBrand = ListBrand1;
            ViewBag.ListClients = ListClients2;

            ViewData["GetContractdetails"] = DateStart;
            ViewData["GetContractdetails"] = DateEnd;
            ViewData["GetContractdetails"] = ListBrand;
            ViewData["GetContractdetails"] = ListClients;

            List<Contract> empquery;

            empquery = db.Contract.ToList();

            if (!String.IsNullOrEmpty(ListBrand) && !String.IsNullOrEmpty(ListClients) && DateStart != null && DateEnd != null)
            {
                empquery = empquery.Where(x => x.Car_Brand.Contains(ListBrand)
                    && x.FIO_Customer.Contains(ListClients)
                    && x.Date_Start <= DateStart
                    && x.Date_End >= DateEnd).ToList();
            }
            else if (!String.IsNullOrEmpty(ListBrand) && !String.IsNullOrEmpty(ListClients))
            {
                empquery = empquery.Where(x => x.Car_Brand.Contains(ListBrand)
                   && x.FIO_Customer.Contains(ListClients)).ToList();
            }
            else if (!String.IsNullOrEmpty(ListBrand))
            {
                empquery = empquery.Where(x => x.Car_Brand.Contains(ListBrand)).ToList();
            }
            else if (!String.IsNullOrEmpty(ListClients))
            {
                empquery = empquery.Where(x => x.FIO_Customer.Contains(ListClients)).ToList();
            }
            else if (DateStart >= DateTime.Parse("01.01.2022") && DateEnd <= DateTime.Now.AddYears(1))
            {
                empquery = empquery.Where(x => x.Date_Start >= DateStart && x.Date_End <= DateEnd).ToList();
            }
            else if (DateStart != null)
            {
                empquery = empquery.Where(x => x.Date_Start >= DateStart).ToList();
            }
            else if (DateEnd != null)
            {
                empquery = empquery.Where(x => x.Date_End <= DateEnd).ToList();
            }

            ViewBag.TotalPages = Math.Ceiling(empquery.Count() / 15.0);
            ViewBag.PageNumber = 1;

            return View("IndexByCondition", empquery);
        }

        // GET: User Contracts
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId().ToString();
            var Contracts = db.Contract.Where(contract => contract.id_client.Equals(userID));

            var contractActive = Contracts.Where(contr => contr.Condition.Equals("Не подтверждён") 
                                                            || contr.Condition.Equals("Подтверждён") 
                                                            || contr.Condition.Equals("Действует")
                                                            || contr.Condition.Equals("Ожидает оплаты штрафа")).FirstOrDefault();

            var ifExsistActiveRent = false;

            if(contractActive != null)
            {
                ifExsistActiveRent = true;
            }

            if(ifExsistActiveRent)
            {
                var Car = db.Car_Tbl.Where(auto => auto.WIN_Number.Equals(contractActive.Car_WIN_Number)).FirstOrDefault();

                ViewBag.CarId = Car.id;
                ViewBag.Img = Car.Image.Substring(2);
                ViewBag.CarBrand = Car.Brand;
                ViewBag.CarModel = Car.Model;
                ViewBag.Caryear = Car.Year_Release;

                ViewBag.DateStart = contractActive.Date_Start;
                ViewBag.DateEnd = contractActive.Date_End;
                ViewBag.Notes = contractActive.Notes;
                ViewBag.Options = contractActive.Additional_Options;
                ViewBag.Condition = contractActive.Condition;
                ViewBag.ContractID = contractActive.id;

                var HowStay = contractActive.Date_End - DateTime.Now;
                var HowDayStay = HowStay.Days;
                var HowHoursStay = HowStay.Hours;
                var HowMinutesStay = HowStay.Minutes;


                ViewBag.DayStay = HowDayStay;
                ViewBag.HoursStay = HowHoursStay;
                ViewBag.HowMinutessStay = HowMinutesStay;
            }
            ViewBag.IfExsistActive = ifExsistActiveRent;


            int lenght = Contracts.Count();
            return View(Contracts.ToList());
        }

        // GET: All Contracts
        public ActionResult IndexAll()
        {
            var cars = db.Car_Tbl.ToList();
            List<String> carsBrand = new List<String>();

            for (int i = 0; i < cars.Count; i++)
            {
                carsBrand.Add(cars[i].Brand.ToString());
            }

            carsBrand = carsBrand.Distinct().ToList();

            List<SelectListItem> ListBrand1 = new List<SelectListItem>();

            for (int i = 0; i < carsBrand.Count; i++)
            {
                ListBrand1.Add(new SelectListItem { Text = carsBrand[i].ToString(), Value = carsBrand[i].ToString() });
            }

            var clients = db.Customer_Tbl.ToList();
            List<String> clientsFIO = new List<String>();


            for (int i = 0; i < clients.Count; i++)
            {
                clientsFIO.Add(clients[i].FIO.ToString());
            }

            clientsFIO = clientsFIO.Distinct().ToList();

            List<SelectListItem> ListClients = new List<SelectListItem>();

            for (int i = 0; i < clientsFIO.Count; i++)
            {
                ListClients.Add(new SelectListItem { Text = clientsFIO[i].ToString(), Value = clientsFIO[i].ToString() });
            }

            ViewBag.ListBrand = ListBrand1;
            ViewBag.ListClients = ListClients;

            return View(db.Contract.ToList());
        }


        public ActionResult IndexByConditionToClient(string condition)
        {
            var userID = User.Identity.GetUserId();
            var contracts = new List<Contract>();
            var IfExsist = false;
            switch (condition)
            {
                case "All":
                    {
                        contracts = db.Contract.ToList().Where(contr => contr.id_client.Equals(userID)).ToList();
                        if (contracts.Count != 0) IfExsist = true;
                        break;
                    }
                case "Отменён":
                    {
                        contracts = db.Contract.Where(contr => contr.Condition.Equals(condition) && contr.id_client.Equals(userID)).ToList();
                        if (contracts.Count != 0) IfExsist = true;
                        break;
                    }
                case "Завершён":
                    {
                        contracts = db.Contract.Where(contr => contr.Condition.Equals(condition) && contr.id_client.Equals(userID)).ToList();
                        if (contracts.Count != 0) IfExsist = true;
                        break;
                    }
            }
            ViewBag.IfExsist = IfExsist;
            return View(contracts);
        }
        // GET: Contracts/Details/51
        public ActionResult Details(int? id)
        {
            Contract contract = db.Contract.Find(id);

            string[] subs = contract.Additional_Options.Split(' ');
            var ifVideoRegistrator = subs.Length >= 1 ? true : false;
            var ifAutoBox = subs.Length >= 2 ? true : false;
            var ifChildChes = subs.Length == 3 ? subs.Last().Last().ToString() : "нет";

            ViewBag.ifVideoRegistrator = ifVideoRegistrator;
            ViewBag.ifAutoBox = ifAutoBox;
            ViewBag.ifChildChes = ifChildChes;

            var user = db.Customer_Tbl.Where(customer => customer.user_ID.Equals(contract.id_client)).First();
            var BirthDate = user.BirthDate;
            var Login = user.Login;
            var Passport = user.Passport_Data;
            var LicenseNumber = user.Drivers_License;
            var Address = user.Address;

            ViewBag.BD = BirthDate;
            ViewBag.Login = Login;
            ViewBag.Passport = Passport;
            ViewBag.LicenseNumber = LicenseNumber;
            ViewBag.Address = Address;

            var car = db.Car_Tbl.Where(auto => auto.WIN_Number.Equals(contract.Car_WIN_Number)).FirstOrDefault();
            if (car != null)
            {
                var Image = car.Image;
                var Brand = car.Brand;
                var Model = car.Model;
                var Year = car.Year_Release;
                var WIN = car.WIN_Number;

                ViewBag.CarId = car.id;
                ViewBag.Image = car.Image;
                ViewBag.Brand = Brand;
                ViewBag.Model = Model;
                ViewBag.Year = Year;
                ViewBag.WIN = WIN;
            }
            else
            {
                ViewBag.CarId = "";
                ViewBag.Image = "../Image/defaulst.jpeg";
                ViewBag.Brand = "Авто был удалён(";
                ViewBag.Model = "";
                ViewBag.Year = "";
                ViewBag.WIN = "Авто был удалён(";
            }

            ViewBag.RentalDate = (contract.Date_End - contract.Date_Start).Duration().ToString("dd");
            if ((contract.Date_End - contract.Date_Start).Hours == 0 && (contract.Date_End - contract.Date_Start).Minutes == 0) {
                ViewBag.RentalHours = null;
            }
            else
            {
                ViewBag.RentalHours = (contract.Date_End - contract.Date_Start).Duration().ToString().Substring(2, 5);
            }
            ViewBag.DateStart = contract.Date_Start.ToString("dd MMMM");
            ViewBag.DateEnd = contract.Date_End.ToString("dd MMMM");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Confirm(int id)
        {
            var contract = db.Contract.Where(model => model.id == id).First();
            contract.Condition = "Подтверждён";

            var managerID = User.Identity.GetUserId();
            var managerFIO = db.Manager_Tbl.Where(manager => manager.user_ID.Equals(managerID)).First().FIO;
            contract.FIO_Manager = managerFIO;

            db.Entry(contract).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("IndexAll");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Canceled(int id)
        {
            var contract = db.Contract.Where(model => model.id == id).First();
            var car = db.Car_Tbl.Where(auto => auto.WIN_Number.Equals(contract.Car_WIN_Number)).FirstOrDefault();

            car.Contition = "Свободна";
            db.Entry(car).State = System.Data.Entity.EntityState.Modified;
            contract.Condition = "Отменён";

            if (User.IsInRole("Manager"))
            {
                var managerID = User.Identity.GetUserId();
                var managerFIO = db.Manager_Tbl.Where(manager => manager.user_ID.Equals(managerID)).First().FIO;
                contract.FIO_Manager = managerFIO;
            }

            db.Entry(contract).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();

            if (User.IsInRole("Manager"))
            {
                return RedirectToAction("IndexAll");
            }

            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Finished(int id)
        {
            var contract = db.Contract.Where(model => model.id == id).First();
            var car = db.Car_Tbl.Where(auto => auto.WIN_Number.Equals(contract.Car_WIN_Number)).FirstOrDefault();
            contract.Condition = "Завершён";
            //contract.Date_End = DateTime.Now;

            car.Contition = "Свободна";
            db.Entry(car).State = System.Data.Entity.EntityState.Modified;
            db.Entry(contract).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            if(User.IsInRole("Client"))
            {
                return RedirectToAction("Index");
            }else
            {
                return RedirectToAction("IndexAll");
            }
        }

        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Start(int id)
        {
            var contract = db.Contract.Where(model => model.id == id).First();
            contract.Condition = "Действует";

            db.Entry(contract).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("IndexAll");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AssignFine(int id)
        {
            var contract = db.Contract.Where(model => model.id == id).First();
            contract.Condition = "Ожидает оплаты штрафа";

            db.Entry(contract).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("IndexAll");
        }

        // GET: Contracts/Create
        public ActionResult Create(int id, string dateStart, string dateEnd)
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
            //double price = (Mode- contract.Date_Start).TotalDays * contract.Price;
            var userID = User.Identity.GetUserId();
            var user = db.Customer_Tbl.Where(usr => usr.user_ID.Equals(userID)).FirstOrDefault();

            ViewBag.IfUserDataExist = user == null;

            if(user != null)
            {
                ViewBag.UserFIO = user.FIO;
            }else
            {
                ViewBag.UserFIO = "";
            }

            ViewBag.DateStart = dateStart;
            ViewBag.DateEnd = dateEnd;

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
                //double price = (contract.Date_End - contract.Date_Start).TotalDays * contract.Price;
                string strCurrentUserId = User.Identity.GetUserId().ToString();
                contract.id_client = strCurrentUserId;
                contract.Car_Brand = car.Brand;
                contract.Car_Model = car.Model;
                contract.Car_WIN_Number = car.WIN_Number;
                contract.Condition = "Не подтверждён";
                //contract.Price = Double.Parse(contract.Price);
                car.Contition = "Забронирована";
                db.Entry(car).State = System.Data.Entity.EntityState.Modified;
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

            var managerID = User.Identity.GetUserId();

            contract.FIO_Manager = db.Manager_Tbl.Where(manager => manager.user_ID.Equals(managerID)).FirstOrDefault().FIO;

            //ViewBag.UserID = contract.id_client;

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
        public ActionResult Edit([Bind(Include = "id,FIO_Customer,FIO_Manager,Car_Brand,Car_Model,Car_WIN_Number,Additional_Options,Date_Start,Date_End,Price,Condition,Notes,id_client")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contract).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAll");
            }
            return RedirectToAction("IndexAll");
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
            return RedirectToAction("IndexAll");
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
