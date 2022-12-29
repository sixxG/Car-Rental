using CarRental.Controllers;
using CarRental.IRep;
using CarRental.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CarRental.Test.Controllers
{
    [TestClass]
    public class CarControllerTest
    {
        [TestMethod]
        public void Index()
        {
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Автопарк", actual);
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void Create()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Добавить авто", actual);
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void CreatePostAction_ModelError()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            Car_Tbl car = new Car_Tbl();

            CarController controller = new CarController(mock.Object);
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            ViewResult result = controller.Create(car) as ViewResult;

            // Assert
            Assert.IsNull(result);
            //Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void CreatePostAction_RedirectToIndexView()
        {
            // Arrange
            string expected = "Index";
            var mock = new Mock<IRepository>();
            Car_Tbl car = new Car_Tbl();
            CarController controller = new CarController(mock.Object);
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            RedirectToRouteResult result = controller.Create(car) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void CreatePostAction_Created()
        {
            // Arrange
            string expected = "Index";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

            Car_Tbl car = new Car_Tbl(0,"TEST","TEST","TEST","TEST","TEST", 2022, 2022, "TEST", "TEST", "TEST", 2022, 2022, "TEST", null, "TEST");
            CarController controller = new CarController();

            // Act
            RedirectToRouteResult result = controller.Create(car) as RedirectToRouteResult;
            //var Created_car = mock.Setup(a => a.GetCasrList().Where(auto => auto.WIN_Number.Equals("TEST")).FirstOrDefault()).Returns(new Car_Tbl());
            var Created_car = db.Car_Tbl.ToList().Where(auto => auto.WIN_Number.Equals("TEST")).FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
            Assert.IsNotNull(Created_car);
            Assert.AreEqual(2022, Created_car.Price_Per_Day);
        }

        [TestMethod]
        public void Edit()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.Edit(52001) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Редактироавть авто", actual);
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void EditPostAction_ModelError()
        {
            // Arrange
            string expected = "Edit";
            var mock = new Mock<IRepository>();

            CarController controller = new CarController(mock.Object);
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            ViewResult result = controller.Edit(52001) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void EditPostAction_RedirectToIndexView()
        {
            // Arrange
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            string expected = "Index";
            var mock = new Mock<IRepository>();
            Car_Tbl car = db.Car_Tbl.Find(52001);
            CarController controller = new CarController(mock.Object);
            //controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            RedirectToRouteResult result = controller.Create(car) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void EditPostAction_Edited()
        {
            // Arrange
            string expected = "Details";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

            Car_Tbl car = db.Car_Tbl.Find(52001);
            var carEdit = car;
            var carPower = car.Power;
            carEdit.Power = carPower + DateTime.Now.Day % 2 + 2;
            CarController controller = new CarController();

            // Act
            RedirectToRouteResult result = controller.Edit(carEdit) as RedirectToRouteResult;
            var Edited_car = db.Car_Tbl.Find(52001);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
            Assert.IsNotNull(Edited_car);
            Assert.AreNotEqual(carPower, Edited_car.Power);
        }

        [TestMethod]
        public void Details()
        {
            // Arrange
            string expected = "Details";
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.Details(52001) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Car Details", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var car = db.Car_Tbl.Where(auto => auto.WIN_Number.Equals("TEST")).FirstOrDefault();
            string expected = "Delete";
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.Delete(car.id) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Car Delete", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void DeletePostAction_Deleted()
        {
            // Arrange
            string expected = "Index";
            var mock = new Mock<IRepository>();
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var car = db.Car_Tbl.Where(auto => auto.WIN_Number.Equals("TEST")).FirstOrDefault();

            CarController controller = new CarController(mock.Object);

            // Act
            RedirectToRouteResult result = controller.DeleteConfirmed(car.id) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
            Assert.IsNotNull(db.Car_Tbl.Find(car.id));
        }

        [TestMethod]
        public void IndexClass_Эконом()
        {
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.IndexClass("Эконом") as ViewResult;
            var actual = result.ViewBag.Message;
            var car = (List<Car_Tbl>)result.Model;

            // 
            Assert.AreEqual("List Car by Class", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("Эконом", car.FirstOrDefault().Class);
        }

        [TestMethod]
        public void IndexClass_All()
        {
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.IndexClass("All") as ViewResult;
            var actual = result.ViewBag.Message;
            var car = (List<Car_Tbl>)result.Model;

            // 
            Assert.AreEqual("List Car by Class", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreNotEqual(car.LastOrDefault().Class, car.FirstOrDefault().Class);
        }

        [TestMethod]
        public void FindCar()
        {
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.FindCar(null, null, null, null, null) as ViewResult;
            var actual = result.ViewBag.Message;
            var car = (List<Car_Tbl>)result.Model;

            // 
            Assert.AreEqual("List Car by Class", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(db.Car_Tbl.Count(), car.Count());
        }

        [TestMethod]
        public void FindCar_All_Сriteria()
        {
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.FindCar(3000, 3500, "Lifan", "Механическая", "Комфорт") as ViewResult;
            var actual = result.ViewBag.Message;
            var car = (List<Car_Tbl>)result.Model;

            // 
            Assert.AreEqual("List Car by Class", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("VF7RD5FNABL500910", car.FirstOrDefault().WIN_Number);
        }

        [TestMethod]
        public void FindCar_Price()
        {
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.FindCar(3000, 3500, null, null, null) as ViewResult;
            var actual = result.ViewBag.Message;
            var car = (List<Car_Tbl>)result.Model;

            // 
            Assert.AreEqual("List Car by Class", actual);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(car.FirstOrDefault().Price_Per_Day >= 3000 && car.FirstOrDefault().Price_Per_Day <= 3500);
        }

        [TestMethod]
        public void FindCar_Brand()
        {
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.FindCar(null, null, "BMW", null, null) as ViewResult;
            var actual = result.ViewBag.Message;
            var car = (List<Car_Tbl>)result.Model;

            // 
            Assert.AreEqual("List Car by Class", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("BMW", car.FirstOrDefault().Brand);
        }

        [TestMethod]
        public void FindCar_Transmition()
        {
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            CarController controller = new CarController(mock.Object);

            // Act
            ViewResult result = controller.FindCar(null, null, null, "Робот", null) as ViewResult;
            var actual = result.ViewBag.Message;
            var car = (List<Car_Tbl>)result.Model;

            // 
            Assert.AreEqual("List Car by Class", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("Робот", car.FirstOrDefault().Type_Transmission);
        }
    }
}
