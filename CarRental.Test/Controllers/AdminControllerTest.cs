using CarRental.Controllers;
using CarRental.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CarRental.Test.Controllers
{
    [TestClass]
    public class AdminControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            AdminController controller = new AdminController();

            // Act
            ViewResult result = controller.Index() as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Admins", actual);
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void Details()
        {
            // Arrange
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            string expected = "Details";
            AdminController controller = new AdminController();

            // Act
            ViewResult result = controller.Details(db.Admin_Tbl.FirstOrDefault().id) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Admin Details", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void Create()
        {
            // Arrange
            AdminController controller = new AdminController();

            // Act
            ViewResult result = controller.Create() as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Create Admin", actual);
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void CreatePostAction_ModelError()
        {
            // Arrange
            Admin_Tbl admin = new Admin_Tbl();
            string expected = "Create";

            AdminController controller = new AdminController();
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            ViewResult result = controller.Create(admin) as ViewResult;

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

            Admin_Tbl admin = new Admin_Tbl("TEST", "TEST", "TEST", "TEST");
            AdminController controller = new AdminController();

            // Act
            ViewResult result = controller.Create(admin) as ViewResult;
            var Created_admin = db.Admin_Tbl.ToList().Where(man => man.FIO.Equals("TEST")).FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
            Assert.IsNotNull(Created_admin);
            Assert.AreEqual("TEST", Created_admin.Login);
        }

        [TestMethod]
        public void Edit()
        {
            // Arrange
            string expected = "Edit";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            AdminController controller = new AdminController();
            var Created_admin = db.Admin_Tbl.ToList().Where(man => man.FIO.Equals("TEST")).FirstOrDefault();

            // Act
            ViewResult result = controller.Edit(Created_admin.user_ID) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Edit Admin", actual);
            Assert.AreEqual(expected, result.ViewName);
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void EditPostAction_ModelError()
        {
            // Arrange
            string expected = "Edit";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var Created_admin = db.Admin_Tbl.ToList().Where(man => man.FIO.Equals("TEST")).FirstOrDefault();

            AdminController controller = new AdminController();
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            ViewResult result = controller.Edit(Created_admin) as ViewResult;
            var actual = result.ViewBag.Message;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
            Assert.AreEqual("Admin was`nt Edited", actual);
        }

        [TestMethod]
        public void EditPostAction_Edited()
        {
            // Arrange
            string expected = "Edit";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();

            var Created_admin = db.Admin_Tbl.ToList().Where(man => man.FIO.Equals("TEST")).FirstOrDefault();
            var FIO_Before = Created_admin.FIO;
            Created_admin.FIO = "OLEG";
            AdminController controller = new AdminController();

            // Act
            ViewResult result = controller.Edit(Created_admin) as ViewResult;
            Created_admin = db.Admin_Tbl.Find(Created_admin.id);
            var actual = result.ViewBag.Message;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
            Assert.IsNotNull(Created_admin);
            Assert.AreNotEqual(FIO_Before, Created_admin.FIO);
            Assert.AreEqual("Admin was Edited", actual);
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var Created_admin = db.Admin_Tbl.ToList().Where(man => man.FIO.Equals("OLEG")).FirstOrDefault();
            string expected = "Delete";
            AdminController controller = new AdminController();

            // Act
            ViewResult result = controller.Delete(Created_admin.id) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Delete Admin", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void DeletePostAction_Deleted()
        {
            // Arrange
            string expected = "Index";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var Created_admin = db.Admin_Tbl.ToList().Where(man => man.FIO.Equals("OLEG")).FirstOrDefault();
            var id = Created_admin.id;

            AdminController controller = new AdminController();

            // Act
            RedirectToRouteResult result = controller.DeleteConfirmed(id) as RedirectToRouteResult;
            CarRentalMVCEntities1 db2 = new CarRentalMVCEntities1();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
            Assert.IsNull(db2.Admin_Tbl.Find(id));
        }
    }
}
