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

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
        private Car_Tbl context = new Car_Tbl();

        // GET: Car
        public ActionResult Index()
        {
            //ViewBag.TotalPages = Math.Ceiling(db.Car_Tbl.ToList().Count() / 10.0);
            return View(db.Car_Tbl.ToList());
        }

        // GET: Car
        public ActionResult IndexClass(string s, int PageNumber = 1)
        {
            var cars = db.Car_Tbl.ToList().Where(x => x.Class.Equals(s));

            ViewBag.TotalPages = Math.Ceiling(cars.Count() / 3.0);
            ViewBag.PageNumber = PageNumber;

            cars = cars.Skip((PageNumber - 1) * 3).Take(3).ToList();

            ViewBag.Class = s;
            return View(cars);
        }

        //Метод для поиска
        [HttpGet]
        public async Task<ActionResult> Index(string Empsearch)
        {
            ViewData["Getcardetails"] = Empsearch;

            var empquery = from x in db.Car_Tbl select x;
            if(!String.IsNullOrEmpty(Empsearch))
            {
                empquery = empquery.Where(x => x.Brand.Contains(Empsearch)
                    || x.Model.Contains(Empsearch));
            }
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
            var listClass = new List<String>() { "Эконом", "Комфорт", "Бизнесс", "Premium", "Внедорожники", "Минивэны", "Уникальные авто"};
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
                        db.Entry(car_Tbl).State = EntityState.Modified;
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

            db.Entry(car_Tbl).State = EntityState.Modified;
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
