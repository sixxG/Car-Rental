using CarRental.Controllers;
using CarRental.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CarRental.Test.Controllers
{
    [TestClass]
    public class ContractsControllerTest
    {
        [TestMethod]
        public void IndexAll()
        {
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            // Arrange
            ContractsController controller = new ContractsController();

            // Act
            ViewResult result = controller.IndexAll() as ViewResult;
            var actual = result.ViewBag.Message;
            string expected = "IndexAll";

            // 
            Assert.AreEqual("Contracts", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void Create()
        {
            // Arrange
            string expected = "Create";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            ContractsController controller = new ContractsController();
            Car_Tbl car = new Car_Tbl(0, "TEST123", "TEST", "TEST", "TEST", "TEST", 2022, 2022, "TEST", "TEST", "TEST", 2022, 2022, "TEST", null, "TEST");
            Customer_Tbl customer = new Customer_Tbl("TEST123", DateTime.Now, "1234 5678", "TEST", "TEST", "TEST", "TEST", "TEST");
            CarController Carcontroller = new CarController();
            CustomerController Customercontroller = new CustomerController();

            // Act
            Carcontroller.Create(car);
            Customercontroller.Create(customer);
            CarRentalMVCEntities1 db2 = new CarRentalMVCEntities1();
            var newCar = db2.Car_Tbl.Find(car.id);
            var newCustomer = db2.Customer_Tbl.Add(customer);

            ViewResult result = controller.Create(newCar.id, DateTime.Now.ToString(), DateTime.Now.AddDays(10).ToString(), newCustomer.user_ID) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Create Contract", actual);
            Assert.IsNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void CreatePostAction_ModelError()
        {
            // Arrange
            string expected = "Create";
            Contract contract = new Contract();
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

            ContractsController controller = new ContractsController();
            controller.ModelState.AddModelError("Name", "Название модели не установлено");
            var car = db.Car_Tbl.Where(auto => auto.WIN_Number.Equals("TEST123")).FirstOrDefault();

            // Act
            ViewResult result = controller.Create(contract, car.id) as ViewResult;
            var actual = result.ViewBag.Message;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
            Assert.AreEqual("Faild!", actual);
        }

        [TestMethod]
        public void CreatePostAction_Created()
        {
            // Arrange
            string expected = "Index";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

            Contract contract = new Contract("TEST123", "TEST","TEST","TEST","TEST123", null, DateTime.Now, DateTime.Now.AddDays(10),
                                             1000000, "Не подтверждён", "TEST CONTRACT", "TEST");
            ContractsController controller = new ContractsController();
            var car = db.Car_Tbl.Where(auto => auto.WIN_Number.Equals("TEST123")).FirstOrDefault();

            // Act
            RedirectToRouteResult result = controller.Create(contract, car.id) as RedirectToRouteResult;
            //var Created_car = mock.Setup(a => a.GetCasrList().Where(auto => auto.WIN_Number.Equals("TEST")).FirstOrDefault()).Returns(new Car_Tbl());
            var Created_contract = db.Contract.Where(cntr => cntr.Car_WIN_Number.Equals("TEST123")).FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
            Assert.IsNotNull(Created_contract);
            Assert.AreEqual("TEST CONTRACT", Created_contract.Notes);
        }

        [TestMethod]
        public void Edit()
        {
            // Arrange
            string expected = "Edit";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            ContractsController controller = new ContractsController();
            var Created_contract = db.Contract.Where(cntr => cntr.Car_WIN_Number.Equals("TEST123")).FirstOrDefault();

            // Act
            ViewResult result = controller.Edit(Created_contract.id) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Edit Contract", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void EditPostAction_RedirectToIndexView()
        {
            // Arrange
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            string expected = "IndexAll";
            var Created_contract = db.Contract.Where(cntr => cntr.Car_WIN_Number.Equals("TEST123")).FirstOrDefault();
            ContractsController controller = new ContractsController();
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            RedirectToRouteResult result = controller.Edit(Created_contract) as RedirectToRouteResult;
            //var actual = result.ViewBag.Message;

            // Assert
            //Assert.AreEqual("Faile Edit!", actual);
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [TestMethod]
        public void EditPostAction_Edited()
        {
            // Arrange
            string expected = "IndexAll";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

            var Created_contract = db.Contract.Where(cntr => cntr.Car_WIN_Number.Equals("TEST123")).FirstOrDefault();
            var Additional_Optionals_Befor = Created_contract.Additional_Options;
            Created_contract.Additional_Options = DateTime.Now.ToString();
            ContractsController controller = new ContractsController();

            // Act
            RedirectToRouteResult result = controller.Edit(Created_contract) as RedirectToRouteResult;
            Created_contract = db.Contract.Find(Created_contract.id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
            Assert.IsNotNull(Created_contract);
            Assert.AreNotEqual(Additional_Optionals_Befor, Created_contract.Additional_Options);
        }

        [TestMethod]
        public void Details()
        {
            // Arrange
            string expected = "Details";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            ContractsController controller = new ContractsController();
            var Created_contract = db.Contract.Where(cntr => cntr.Car_WIN_Number.Equals("TEST123")).FirstOrDefault();

            // Act
            ViewResult result = controller.Details(Created_contract.id) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Contract Details", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void Index()
        {
            // Arrange
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            ContractsController controller = new ContractsController();
            var Created_contract = db.Contract.Where(cntr => cntr.Car_WIN_Number.Equals("TEST123")).FirstOrDefault();
            var user = db.Customer_Tbl.Where(client => client.user_ID.Equals(Created_contract.id_client)).FirstOrDefault();

            // Act
            ViewResult result = controller.Index(user.user_ID) as ViewResult;
            var actual = result.ViewBag.Message;
            var contract = (List<Contract>)result.Model;

            // 
            Assert.AreEqual("List Contracts to Customer", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(user.FIO, contract.FirstOrDefault().FIO_Customer);
        }

        //[TestMethod]
        //public void Delete()
        //{
        //    // Arrange
        //    CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
        //    var car = db.Car_Tbl.Where(auto => auto.WIN_Number.Equals("TEST")).FirstOrDefault();
        //    string expected = "Delete";
        //    var mock = new Mock<IRepository>();
        //    mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
        //    CarController controller = new CarController(mock.Object);

        //    // Act
        //    ViewResult result = controller.Delete(car.id) as ViewResult;
        //    var actual = result.ViewBag.Message;

        //    // 
        //    Assert.AreEqual("Car Delete", actual);
        //    Assert.IsNotNull(result.Model);
        //    Assert.AreEqual(expected, result.ViewName);
        //}

        //[TestMethod]
        //public void DeletePostAction_Deleted()
        //{
        //    // Arrange
        //    string expected = "Index";
        //    var mock = new Mock<IRepository>();
        //    CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
        //    var car = db.Car_Tbl.Where(auto => auto.WIN_Number.Equals("TEST")).FirstOrDefault();

        //    CarController controller = new CarController(mock.Object);

        //    // Act
        //    RedirectToRouteResult result = controller.DeleteConfirmed(car.id) as RedirectToRouteResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(expected, result.RouteValues["action"]);
        //    Assert.IsNotNull(db.Car_Tbl.Find(car.id));
        //}

        //[TestMethod]
        //public void FindCar()
        //{
        //    CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
        //    // Arrange
        //    var mock = new Mock<IRepository>();
        //    mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
        //    CarController controller = new CarController(mock.Object);

        //    // Act
        //    ViewResult result = controller.FindCar(null, null, null, null, null) as ViewResult;
        //    var actual = result.ViewBag.Message;
        //    var car = (List<Car_Tbl>)result.Model;

        //    // 
        //    Assert.AreEqual("List Car by Class", actual);
        //    Assert.IsNotNull(result.Model);
        //    Assert.AreEqual(db.Car_Tbl.Count(), car.Count());
        //}

        //[TestMethod]
        //public void FindCar_All_Сriteria()
        //{
        //    CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
        //    // Arrange
        //    var mock = new Mock<IRepository>();
        //    mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
        //    CarController controller = new CarController(mock.Object);

        //    // Act
        //    ViewResult result = controller.FindCar(3000, 3500, "Lifan", "Механическая", "Комфорт") as ViewResult;
        //    var actual = result.ViewBag.Message;
        //    var car = (List<Car_Tbl>)result.Model;

        //    // 
        //    Assert.AreEqual("List Car by Class", actual);
        //    Assert.IsNotNull(result.Model);
        //    Assert.AreEqual("VF7RD5FNABL500910", car.FirstOrDefault().WIN_Number);
        //}

        //[TestMethod]
        //public void FindCar_Price()
        //{
        //    CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
        //    // Arrange
        //    var mock = new Mock<IRepository>();
        //    mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
        //    CarController controller = new CarController(mock.Object);

        //    // Act
        //    ViewResult result = controller.FindCar(3000, 3500, null, null, null) as ViewResult;
        //    var actual = result.ViewBag.Message;
        //    var car = (List<Car_Tbl>)result.Model;

        //    // 
        //    Assert.AreEqual("List Car by Class", actual);
        //    Assert.IsNotNull(result.Model);
        //    Assert.IsTrue(car.FirstOrDefault().Price_Per_Day >= 3000 && car.FirstOrDefault().Price_Per_Day <= 3500);
        //}

        //[TestMethod]
        //public void FindCar_Brand()
        //{
        //    CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
        //    // Arrange
        //    var mock = new Mock<IRepository>();
        //    mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
        //    CarController controller = new CarController(mock.Object);

        //    // Act
        //    ViewResult result = controller.FindCar(null, null, "BMW", null, null) as ViewResult;
        //    var actual = result.ViewBag.Message;
        //    var car = (List<Car_Tbl>)result.Model;

        //    // 
        //    Assert.AreEqual("List Car by Class", actual);
        //    Assert.IsNotNull(result.Model);
        //    Assert.AreEqual("BMW", car.FirstOrDefault().Brand);
        //}

        //[TestMethod]
        //public void FindCar_Transmition()
        //{
        //    CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
        //    // Arrange
        //    var mock = new Mock<IRepository>();
        //    mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
        //    CarController controller = new CarController(mock.Object);

        //    // Act
        //    ViewResult result = controller.FindCar(null, null, null, "Робот", null) as ViewResult;
        //    var actual = result.ViewBag.Message;
        //    var car = (List<Car_Tbl>)result.Model;

        //    // 
        //    Assert.AreEqual("List Car by Class", actual);
        //    Assert.IsNotNull(result.Model);
        //    Assert.AreEqual("Робот", car.FirstOrDefault().Type_Transmission);
        //}
    }
}
