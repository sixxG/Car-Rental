using CarRental.Controllers;
using CarRental.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CarRental.Test.Controllers
{
    [TestClass]
    public class CustomerControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            CustomerController controller = new CustomerController();

            // Act
            ViewResult result = controller.Index() as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Customers", actual);
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void Details()
        {
            // Arrange
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            string expected = "Details";
            CustomerController controller = new CustomerController();

            // Act
            ViewResult result = controller.Details(db.Customer_Tbl.FirstOrDefault().Id) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Customer Details", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void Create()
        {
            // Arrange
            CustomerController controller = new CustomerController();

            // Act
            ViewResult result = controller.Create() as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Create Customer", actual);
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void CreatePostAction_ModelError()
        {
            // Arrange
            Customer_Tbl customer = new Customer_Tbl();
            string expected = "Create";

            CustomerController controller = new CustomerController();
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            ViewResult result = controller.Create(customer) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void CreatePostAction_Created()
        {
            // Arrange
            string expected = "Create";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

            Customer_Tbl customer = new Customer_Tbl("TEST", DateTime.Now , "1234 5678", "TEST", "TEST", "TEST", "TEST", "TEST");
            CustomerController controller = new CustomerController();

            // Act
            ViewResult result = controller.Create(customer) as ViewResult;
            var Created_customer = db.Customer_Tbl.ToList().Where(cust => cust.FIO.Equals("TEST")).FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
            Assert.IsNotNull(Created_customer);
            Assert.AreEqual("1234 5678", Created_customer.Passport_Data);
        }

        [TestMethod]
        public void Edit()
        {
            // Arrange
            string expected = "Edit";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            CustomerController controller = new CustomerController();
            var Created_customer = db.Customer_Tbl.ToList().Where(cust => cust.FIO.Equals("TEST")).FirstOrDefault();

            // Act
            ViewResult result = controller.Edit(Created_customer.user_ID) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Edit Customer", actual);
            Assert.AreEqual(expected, result.ViewName);
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void EditPostAction_ModelError()
        {
            // Arrange
            string expected = "Edit";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var Created_customer = db.Customer_Tbl.ToList().Where(cust => cust.FIO.Equals("TEST")).FirstOrDefault();

            CustomerController controller = new CustomerController();
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            ViewResult result = controller.Edit(Created_customer) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void EditPostAction_Edited()
        {
            // Arrange
            string expected = "Edit";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

            var Created_customer = db.Customer_Tbl.ToList().Where(cust => cust.FIO.Equals("TEST")).FirstOrDefault();
            var BirthDateBefor = Created_customer.BirthDate;
            Created_customer.BirthDate = DateTime.Now.AddDays(1);
            CustomerController controller = new CustomerController();

            // Act
            ViewResult result = controller.Edit(Created_customer) as ViewResult;
            Created_customer = db.Customer_Tbl.Find(Created_customer.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
            Assert.IsNotNull(Created_customer);
            Assert.AreNotEqual(BirthDateBefor, Created_customer.BirthDate);
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var Created_customer = db.Customer_Tbl.ToList().Where(cust => cust.FIO.Equals("TEST")).FirstOrDefault();
            string expected = "Delete";
            CustomerController controller = new CustomerController();

            // Act
            ViewResult result = controller.Delete(Created_customer.Id) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Delete Customer", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void DeletePostAction_Deleted()
        {
            // Arrange
            string expected = "Index";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var Created_customer = db.Customer_Tbl.ToList().Where(cust => cust.FIO.Equals("TEST")).FirstOrDefault();
            var id = Created_customer.Id;

            CustomerController controller = new CustomerController();

            // Act
            RedirectToRouteResult result = controller.DeleteConfirmed(id) as RedirectToRouteResult;
            CarRentalMVCEntities1 db2 = new CarRentalMVCEntities1();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
            Assert.IsNull(db2.Customer_Tbl.Find(id));
        }

    }
}
