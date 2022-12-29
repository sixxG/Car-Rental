using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CarRental.IRep;

namespace CarRental.Controllers
{
    public class HomeController : Controller
    {

        IRepository repo;

        public HomeController(IRepository r)
        {
            repo = r;
        }

        public HomeController()
        {
            repo = new CarRepository();
        }
        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
        private CarRepository carRepository;
        private CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

        public ActionResult Index()
        {
            //var listType_Transmission = new SelectList("Механическая", "Автоматическая", "Робот"); /*new List<String>() { "Механическая", "Автоматическая", "Робот" };*/

            //ViewBag.listType_Transmission = new SelectList("Механическая", "Автоматическая", "Робот");

            var cars = db.Car_Tbl.ToList();
            List<String> carsBrand = new List<String>();

            for(int i = 0; i < cars.Count; i++)
            {
                carsBrand.Add(cars[i].Brand.ToString());
            }

            carsBrand = carsBrand.Distinct().ToList();

            List<SelectListItem> ListBrand = new List<SelectListItem>();

            for(int i = 0; i < carsBrand.Count; i++)
            {
                ListBrand.Add(new SelectListItem { Text = carsBrand[i].ToString(), Value = carsBrand[i].ToString() });
            }

            List<SelectListItem> ListTypeTransmition = new List<SelectListItem>();

            ListTypeTransmition.Add(new SelectListItem { Text = "Механическая", Value = "Механическая" });
            ListTypeTransmition.Add(new SelectListItem { Text = "Автоматическая", Value = "Автоматическая" });
            ListTypeTransmition.Add(new SelectListItem { Text = "Робот", Value = "Робот" });

            ViewBag.ListBrand = ListBrand;
            ViewBag.ListTypeTransmition = ListTypeTransmition;

            var popularCars = db.Car_Tbl.Where(car => car.Price_Per_Day >= 4000 && car.Contition.Equals("Свободна")).Take(3);
            return View(popularCars);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Информация";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Контакты";

            return View();
        }
    }
}