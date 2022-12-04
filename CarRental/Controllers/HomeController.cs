using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    public class HomeController : Controller
    {

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

            var popularCars = db.Car_Tbl.Where(car => car.Price_Per_Day >= 3500).Take(3);
            return View(popularCars);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}