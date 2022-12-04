using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CarRental.Models;
using PagedList.Mvc;
using PagedList;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using log4net;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
        private Car_Tbl context = new Car_Tbl();

        //log4net
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //log4net

        // GET: Car
        public ActionResult Index()
        {
            Log.Info("Начал выполняться метод Index контроллера CarController");
            string strCurrentUserId = User.Identity.GetUserId().ToString();
            var userRole = User.IsInRole("Client");
            if (userRole)
            {
                return View(db.Car_Tbl.Where(car => car.Contition.Equals("Свободна")).ToList());
            } else
            {
                return View(db.Car_Tbl.ToList());
            }
        }

        public ActionResult FindCar(int? PriceOT, int? PriceDO, string ListBrand, string ListTypeTransmition)
        {
            ViewData["Getcardetails"] = PriceOT;
            ViewData["Getcardetails"] = PriceDO;
            ViewData["Getcardetails"] = ListBrand;
            ViewData["Getcardetails"] = ListTypeTransmition;


            var empquery = from x in db.Car_Tbl select x;
            if (!String.IsNullOrEmpty(ListBrand) && !String.IsNullOrEmpty(ListTypeTransmition) && PriceDO != null && PriceOT != null)
            {
                empquery = empquery.Where(x => x.Brand.Contains(ListBrand)
                    && x.Type_Transmission.Contains(ListTypeTransmition)
                    && x.Price_Per_Day <= PriceDO
                    && x.Price_Per_Day >= PriceOT);
            } else if (!String.IsNullOrEmpty(ListBrand) && !String.IsNullOrEmpty(ListTypeTransmition))
            {
                empquery = empquery.Where(x => x.Brand.Contains(ListBrand)
                   && x.Type_Transmission.Contains(ListTypeTransmition));
            } else if (!String.IsNullOrEmpty(ListBrand))
            {
                empquery = empquery.Where(x => x.Brand.Contains(ListBrand));
            } else if (PriceOT >= 0 && PriceDO <= 100000)
            {
                empquery = empquery.Where(x => x.Price_Per_Day >= PriceOT && x.Price_Per_Day <= PriceOT);
            }
            int count = empquery.Count();

            return View("Index", empquery.ToList());
        }

        // GET: Car
        public ActionResult IndexClass(string s, int PageNumber = 1)
        {
            Log.Info("Начал выполняться метод IndexClass контроллера CarController");
            var cars = db.Car_Tbl.ToList().Where(x => x.Class.Equals(s));

            ViewBag.TotalPages = Math.Ceiling(cars.Count() / 15.0);
            ViewBag.PageNumber = PageNumber;

            cars = cars.Skip((PageNumber - 1) * 15).Take(15).ToList();

            ViewBag.Class = s;
            Log.Info("Закончил выполняться метод IndexClass контроллера CarController");
            return View(cars);
        }

        //Метод для поиска
        [HttpGet]
        public async Task<ActionResult> Index(string Empsearch)
        {
            Log.Info("Начал выполняться метод Index(string Empsearch) контроллера CarController");
            ViewData["Getcardetails"] = Empsearch;

            var empquery = from x in db.Car_Tbl select x;
            if(!String.IsNullOrEmpty(Empsearch))
            {
                empquery = empquery.Where(x => x.Brand.Contains(Empsearch)
                    || x.Model.Contains(Empsearch));
            }
            Log.Info("Закончил выполняться метод Index(string Empsearch) контроллера CarController");
            return View(await empquery.AsNoTracking().ToListAsync());
        }

        // GET: Car/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car_Tbl car_Tbl = db.Car_Tbl.Find(id);
            if (car_Tbl == null)
            {
                return HttpNotFound();
            }
            return View(car_Tbl);
        }

        // GET: Car/Create
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Create(int id = 0)
        {
            Car_Tbl car = new Car_Tbl();
            var listType_Drive = new List<String>() { "Передний", "Задний", "Полный"};
            var listType_Body = new List<String>() { "Купэ", "Универсал", "Кабриолет", "Седан", "Лимузин", "Внедорожник", "Хетчбэк", "Пикап", "Мини-вэн" };
            var listClass = new List<String>() { "Эконом", "Комфорт", "Бизнес", "Premium", "Внедорожники", "Минивэны", "Уникальные авто"};
            var listType_Transmission = new List<String>() { "Механическая", "Автоматическая", "Робот" };
            ViewBag.listType_Drive = listType_Drive;
            ViewBag.listType_Body = listType_Body;
            ViewBag.listClass = listClass;
            ViewBag.listType_Transmission = listType_Transmission;

            if(id != 0)
            {
                using (CarRentalMVCEntities1 db = new CarRentalMVCEntities1())
                {
                    car = db.Car_Tbl.Where(x => x.id == id).FirstOrDefault<Car_Tbl>();
                }
            }

            return View(car);
        }

        // POST: Car/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Car_Tbl car_Tbl)// add this 
        {
            if (car_Tbl.Image != null && car_Tbl.id == 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(car_Tbl.ImageFile.FileName);
                string extension = Path.GetExtension(car_Tbl.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("-yy-MM-dd-HH") + extension;

                car_Tbl.Image = "../Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("../Image/"), fileName);
                car_Tbl.ImageFile.SaveAs(fileName);
            }

            using (CarRentalMVCEntities1 carRentalMVC = new CarRentalMVCEntities1())
            {
                try
                {
                    if(car_Tbl.id == 0)
                    {
                        carRentalMVC.Car_Tbl.Add(car_Tbl);
                        carRentalMVC.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.Entry(car_Tbl).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                            Console.WriteLine("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        //public ActionResult Create(Car_Tbl car_Tbl)
        //{
        //    var selectlist = new SelectList("Передний", "Полный");
        //    ViewBag.selectlist = selectlist;
        //    if (ModelState.IsValid)
        //    {
        //        //string filename = Path.GetFileName(uploadImage.FileName);
        //        //string imgpath = Path.Combine(Server.MapPath("~/Imag/"), filename);
        //        //uploadImage.SaveAs(imgpath);

        //        db.Car_Tbl.Add(car_Tbl);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(car_Tbl);
        //}

        // GET: Car/Edit/5
        public ActionResult Edit(int? id)
        {
            var listType_Drive = new List<String>() { "Передний", "Задний", "Полный" };
            var listType_Body = new List<String>() { "Купэ", "Универсал", "Кабриолет", "Седан", "Лимузин", "Внедорожник", "Хетчбэк", "Пикап", "Мини-вэн" };
            var listClass = new List<String>() { "Эконом", "Комфорт", "Бизнесс", "Premium", "Внедорожники", "Минивэны", "Уникальные авто" };
            var listType_Transmission = new List<String>() { "Механическая", "Автоматическая", "Робот" };
            ViewBag.listType_Drive = listType_Drive;
            ViewBag.listType_Body = listType_Body;
            ViewBag.listClass = listClass;
            ViewBag.listType_Transmission = listType_Transmission;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car_Tbl car_Tbl = db.Car_Tbl.Find(id);
            if (car_Tbl == null)
            {
                return HttpNotFound();
            }
            return View(car_Tbl);
        }

        // POST: Car/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car_Tbl car_Tbl)
        {
            string fileName = Path.GetFileNameWithoutExtension(car_Tbl.ImageFile.FileName);
            string extension = Path.GetExtension(car_Tbl.ImageFile.FileName);

            fileName = fileName + DateTime.Now.ToString("-yy-MM-dd-HH") + extension;
            car_Tbl.Image = "../Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("../Image/"), fileName);
            car_Tbl.ImageFile.SaveAs(fileName);

            db.Entry(car_Tbl).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", car_Tbl.id);
            //return View(car_Tbl);
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car_Tbl car_Tbl = db.Car_Tbl.Find(id);
            if (car_Tbl == null)
            {
                return HttpNotFound();
            }
            return View(car_Tbl);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car_Tbl car_Tbl = db.Car_Tbl.Find(id);
            db.Car_Tbl.Remove(car_Tbl);
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
