using CarRental.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using Moq;
using System.Collections.Generic;
using CarRental.Models;
using CarRental.IRep;
using System.Linq;

namespace CarRental.Test
{
    [TestClass]
    public class HomeControllerTest
    {

        [TestMethod]
        public void Index()
        {
            CarRentalMVCEntities1 db = new CarRentalMVCEntities1();
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetCasrList()).Returns(new List<Car_Tbl>());
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            var actual = result.ViewBag.Message;
            Assert.IsNotNull(result);
            Assert.AreEqual("Информация", actual);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            var actual = result.ViewBag.Message;
            Assert.IsNotNull(result);
            Assert.AreEqual("Контакты", actual);
        }

    }
}
