using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PartyInvites.Domain.Abstract;
using PartyInvites.Domain.Entities;
using PartyInvites.WebUI.Controllers;
using PartyInvites.WebUI.Models;

namespace PartyInvites.UnitTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void Get_All_Guests_No_Filter() {
            //Arrange
            Mock<IGuestRepository> mock = new Mock<IGuestRepository>();
            mock.Setup(g => g.GuestResponses).Returns(new List<GuestResponse>() {
                new GuestResponse() {
                    Name = "Jeffrey Oomen",
                    Email = "jeffrey-140@hotmail.com",
                    Phone = "0637425784",
                    WillAttend = true
                },
                new GuestResponse() {
                    Name = "Dirk Derrex",
                    Email = "dirkderrex@gmail.com",
                    Phone = "0637425784",
                    WillAttend = false
                },
                new GuestResponse() {
                    Name = "Nel Fietsbel",
                    Email = "nelfietsbel@gmail.com",
                    Phone = "0637425784",
                    WillAttend = true
                }
            });

            HomeController controller = new HomeController(mock.Object);

            //Act
            List<GuestResponse> guests = ((GuestListViewModel)controller.GetGuests().Model).GuestResponses.ToList();

            //Assert
            Assert.AreEqual(3, guests.Count());
            Assert.AreEqual("dirkderrex@gmail.com", guests[0].Email);
            Assert.AreEqual("jeffrey-140@hotmail.com", guests[1].Email);
            Assert.AreEqual("nelfietsbel@gmail.com", guests[2].Email);
        }

        [TestMethod]
        public void Get_All_Guests_Present_Filter() {
            //Arrange
            Mock<IGuestRepository> mock = new Mock<IGuestRepository>();


               
            mock.Setup(g => g.GuestResponses).Returns(new List<GuestResponse> {
                new GuestResponse() {
                    Name = "Jeffrey Oomen",
                    Email = "jeffrey-140@hotmail.com",
                    Phone = "0637425784",
                    WillAttend = true
                },
                new GuestResponse() {
                    Name = "Dirk Derrex",
                    Email = "dirkderrex@gmail.com",
                    Phone = "0637425784",
                    WillAttend = false
                },
                new GuestResponse() {
                    Name = "Nel Fietsbel",
                    Email = "nelfietsbel@gmail.com",
                    Phone = "0637425784",
                    WillAttend = true
                }
            });

            HomeController controller = new HomeController(mock.Object);

            //Act
            List<GuestResponse> guests = ((GuestListViewModel)controller.GetGuests("True").Model).GuestResponses.ToList();

            //Assert
            Assert.AreEqual(guests.Count(), 2);
            Assert.AreEqual(guests[0].Email, "jeffrey-140@hotmail.com");
            Assert.AreEqual(guests[1].Email, "nelfietsbel@gmail.com");
        }

        [TestMethod]
        public void Get_All_Guests_Not_Present_Filter() {
            //Arrange
            Mock<IGuestRepository> mock = new Mock<IGuestRepository>();
            mock.Setup(g => g.GuestResponses).Returns(new List<GuestResponse>() {
                new GuestResponse() {
                    Name = "Jeffrey Oomen",
                    Email = "jeffrey-140@hotmail.com",
                    Phone = "0637425784",
                    WillAttend = true
                },
                new GuestResponse() {
                    Name = "Dirk Derrex",
                    Email = "dirkderrex@gmail.com",
                    Phone = "0637425784",
                    WillAttend = false
                },
                new GuestResponse() {
                    Name = "Sanne Oostmaal",
                    Email = "sanneoostmaal@yahoo.com",
                    Phone = "0637425784",
                    WillAttend = false
                },
                new GuestResponse() {
                    Name = "Nel Fietsbel",
                    Email = "nelfietsbel@gmail.com",
                    Phone = "0637425784",
                    WillAttend = true
                }
            });

            HomeController controller = new HomeController(mock.Object);

            //Act
            List<GuestResponse> guests = ((GuestListViewModel)controller.GetGuests("False").Model).GuestResponses.ToList();

            //Assert
            Assert.AreEqual(guests.Count(), 2);
            Assert.AreEqual(guests[0].Email, "dirkderrex@gmail.com");
            Assert.AreEqual(guests[1].Email, "sanneoostmaal@yahoo.com");
        }

        [TestMethod]
        public void Can_Retrieve_Image_Data() {
            // Arrange - create a Guest with image data
            GuestResponse guest = new GuestResponse() {
                Name = "Jeffrey Oomen",
                Email = "jeffrey-140@hotmail.com",
                Phone = "0637425784",
                WillAttend = true,
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Arrange - create the mock repository
            Mock<IGuestRepository> mock = new Mock<IGuestRepository>();
            mock.Setup(m => m.GuestResponses).Returns(new GuestResponse[] {
                new GuestResponse {Name = "Joost Post", Email = "joostpost@gmail.com"},
                guest,
                new GuestResponse {Name = "Cindy Erggo", Email = "cindyerggo@hotmail.com"}
                }.AsQueryable());

            // Arrange - create the controller
            HomeController controller = new HomeController(mock.Object);

            // Act - call the GetImage action method
            ActionResult result = controller.GetImage("jeffrey-140@hotmail.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(guest.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_Email() {
            // Arrange - create the mock repository
            Mock<IGuestRepository> mock = new Mock<IGuestRepository>();
            mock.Setup(m => m.GuestResponses).Returns(new GuestResponse[] {
               new GuestResponse {Name = "Joost Post", Email = "joostpost@gmail.com"},
               new GuestResponse {Name = "Cindy Erggo", Email = "cindyerggo@hotmail.com"}
                }.AsQueryable());

            // Arrange - create the controller
            HomeController controller = new HomeController(mock.Object);

            // Act - call the GetImage action method
            ActionResult result = controller.GetImage("jeffrey-140@hotmail.com");

            // Assert
            Assert.IsNull(result);
        }
    }
}
