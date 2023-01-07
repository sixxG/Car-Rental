using CarRental.Controllers;
using CarRental.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CarRental.Test.Controllers
{
    [TestClass]
    public class ManagerControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            ManagerController controller = new ManagerController();

            // Act
            ViewResult result = controller.Index() as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Managers", actual);
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void Details()
        {
            // Arrange
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            string expected = "Details";
            ManagerController controller = new ManagerController();

            // Act
            ViewResult result = controller.Details(db.Manager_Tbl.FirstOrDefault().id) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Manager Details", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void Create()
        {
            // Arrange
            ManagerController controller = new ManagerController();

            // Act
            ViewResult result = controller.Create() as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Create Manager", actual);
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void CreatePostAction_ModelError()
        {
            // Arrange
            Manager_Tbl manager = new Manager_Tbl();
            string expected = "Create";

            ManagerController controller = new ManagerController();
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            ViewResult result = controller.Create(manager) as ViewResult;

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

            Manager_Tbl manager = new Manager_Tbl("TEST", "TEST", "TEST", "TEST");
            ManagerController controller = new ManagerController();

            // Act
            ViewResult result = controller.Create(manager) as ViewResult;
            var Created_manager = db.Manager_Tbl.ToList().Where(man => man.FIO.Equals("TEST")).FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
            Assert.IsNotNull(Created_manager);
            Assert.AreEqual("TEST", Created_manager.Login);
        }

        [TestMethod]
        public void Edit()
        {
            // Arrange
            string expected = "Edit";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            ManagerController controller = new ManagerController();
            var Created_manager = db.Manager_Tbl.ToList().Where(man => man.FIO.Equals("TEST")).FirstOrDefault();

            // Act
            ViewResult result = controller.Edit(Created_manager.user_ID) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Edit Manager", actual);
            Assert.AreEqual(expected, result.ViewName);
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void EditPostAction_ModelError()
        {
            // Arrange
            string expected = "Edit";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var Created_manager = db.Manager_Tbl.ToList().Where(man => man.FIO.Equals("TEST")).FirstOrDefault();

            ManagerController controller = new ManagerController();
            controller.ModelState.AddModelError("Name", "Название модели не установлено");

            // Act
            ViewResult result = controller.Edit(Created_manager) as ViewResult;

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

            var Created_manager = db.Manager_Tbl.ToList().Where(man => man.FIO.Equals("TEST")).FirstOrDefault();
            var FIO_Before = Created_manager.FIO;
            Created_manager.FIO = "OLEG";
            ManagerController controller = new ManagerController();

            // Act
            ViewResult result = controller.Edit(Created_manager) as ViewResult;
            Created_manager = db.Manager_Tbl.Find(Created_manager.id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
            Assert.IsNotNull(Created_manager);
            Assert.AreNotEqual(FIO_Before, Created_manager.FIO);
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var Created_manager = db.Manager_Tbl.ToList().Where(man => man.Login.Equals("TEST")).FirstOrDefault();
            string expected = "Delete";
            ManagerController controller = new ManagerController();

            // Act
            ViewResult result = controller.Delete(Created_manager.id) as ViewResult;
            var actual = result.ViewBag.Message;

            // 
            Assert.AreEqual("Delete Manager", actual);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void DeletePostAction_Deleted()
        {
            // Arrange
            string expected = "Index";
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            var Created_manager = db.Manager_Tbl.ToList().Where(man => man.Login.Equals("TEST")).FirstOrDefault();
            var id = Created_manager.id;

            ManagerController controller = new ManagerController();

            // Act
            RedirectToRouteResult result = controller.DeleteConfirmed(id) as RedirectToRouteResult;
            CarRentalMVCEntities1 db2 = new CarRentalMVCEntities1();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
            Assert.IsNull(db2.Manager_Tbl.Find(id));
        }
    }
}
