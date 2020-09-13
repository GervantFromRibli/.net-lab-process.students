using System;
using System.Collections.Generic;
using BuisnessLayer;
using DataAccessLayer;
using DomainEntities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;

namespace TicketManagementTests.Unit_Tests
{
    // Class for testing methods that works with LayoutSqlRepository in BLL
    [TestFixture]
    public class BLLLayoutTests
    {
        [TestCase(300, "First name", 100, "First description")]
        [TestCase(120, "Second name", 140, "Second description")]
        [TestCase(300, "Third name", 45, "Third description")]
        public void AddLayout_WhenDataIsValid_ShouldBeEquivalent(int id, string name, int venueId, string description)
        {
            // Arrange
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var layout = new LayoutSqlRepository();
            PrivateObject privateLayoutObject = new PrivateObject(layout);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layout, new SeatSqlRepository(), venueMock.Object);

            // Act
            buisness_layer.AddLayout(id, name, venueId, description);

            // Assert
            var layouts = (List<Layout>)privateLayoutObject.GetField("_layouts");
            layouts[0].Should().BeEquivalentTo(new Layout(id, name, venueId, description));
        }

        [TestCase(300, "First name", 100, "First description")]
        [TestCase(120, "Second name", 140, "Second description")]
        [TestCase(300, "Third name", 45, "Third description")]
        public void AddLayout_WhenDataIsNotValid_ShouldThrowException(int id, string name, int venueId, string description)
        {
            // Arrange
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var layout = new LayoutSqlRepository();
            PrivateObject privateLayoutObject = new PrivateObject(layout);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layout, new SeatSqlRepository(), venueMock.Object);

            // Assert
            buisness_layer.Invoking(elem => elem.AddLayout(id, name + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", venueId, description)).Should().Throw<Exception>().WithMessage("Can`t add a layout");
        }

        [TestCase(300, "First name", 100, "First description")]
        [TestCase(120, "Second name", 140, "Second description")]
        [TestCase(300, "Third name", 45, "Third description")]
        public void AddLayout_WhenDataIsRepeatable_ShouldThrowException(int id, string name, int venueId, string description)
        {
            // Arrange
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var layout = new LayoutSqlRepository();
            PrivateObject privateLayoutObject = new PrivateObject(layout);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layout, new SeatSqlRepository(), venueMock.Object);

            // Act
            buisness_layer.AddLayout(id, name, venueId, description);

            // Assert
            buisness_layer.Invoking(elem => elem.AddLayout(id + 1, name, venueId, description)).Should().Throw<Exception>().WithMessage("There is already a layout with such venue");
        }

        [TestCase(300, "First name", 100, "First description")]
        [TestCase(120, "Second name", 140, "Second description")]
        [TestCase(300, "Third name", 45, "Third description")]
        public void ReadLayout_WhenDataIsValid_ShouldReturnEventAreaThatEquivalent(int id, string name, int venueId, string description)
        {
            // Arrange
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var layout = new LayoutSqlRepository();
            PrivateObject privateLayoutObject = new PrivateObject(layout);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layout, new SeatSqlRepository(), venueMock.Object);
            buisness_layer.AddLayout(id, name, venueId, description);

            // Act
            var seatCheck = buisness_layer.ReadLayout(id);

            // Assert
            seatCheck.Should().BeEquivalentTo(new Layout(id, name, venueId, description));
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(300)]
        public void ReadLayout_WhenDataIsNotValid_ShouldReturnNull(int id)
        {
            // Arrange
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var layout = new LayoutSqlRepository();
            PrivateObject privateLayoutObject = new PrivateObject(layout);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layout, new SeatSqlRepository(), venueMock.Object);
            buisness_layer.AddLayout(id, "Some name", 100, "Some description");

            // Act
            var seatCheck = buisness_layer.ReadLayout(id + 1);

            // Assert
            seatCheck.Should().BeNull();
        }

        [TestCase(300, "First name", 100, "First description")]
        [TestCase(120, "Second name", 140, "Second description")]
        [TestCase(300, "Third name", 45, "Third description")]
        public void UpdateLayout_WhenDataIsValid_ShouldNotBeEquivalent(int id, string name, int venueId, string description)
        {
            // Arrange
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var layout = new LayoutSqlRepository();
            PrivateObject privateLayoutObject = new PrivateObject(layout);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layout, new SeatSqlRepository(), venueMock.Object);
            buisness_layer.AddLayout(id, name, venueId, description);

            // Act
            buisness_layer.UpdateLayout(id, name + "aaa", venueId, description + "aaa");

            // Assert
            var layouts = (List<Layout>)privateLayoutObject.GetField("_layouts");
            layouts[0].Should().NotBeEquivalentTo(new Layout(id, name, venueId, description));
        }

        [TestCase(300, "First name", 100, "First description")]
        [TestCase(120, "Second name", 140, "Second description")]
        [TestCase(300, "Third name", 45, "Third description")]
        public void UpdateLayout_WhenDataIsNotValid_ShouldThrowException(int id, string name, int venueId, string description)
        {
            // Arrange
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var layout = new LayoutSqlRepository();
            PrivateObject privateLayoutObject = new PrivateObject(layout);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layout, new SeatSqlRepository(), venueMock.Object);

            // Act
            buisness_layer.AddLayout(id, name, venueId, description);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateLayout(id, name + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", venueId, description)).Should().Throw<Exception>().WithMessage("Can`t update a layout");
        }

        [TestCase(300, "First name", 100, "First description")]
        [TestCase(120, "Second name", 140, "Second description")]
        [TestCase(300, "Third name", 45, "Third description")]
        public void UpdateLayout_WhenDataIsRepeatable_ShouldThrowException(int id, string name, int venueId, string description)
        {
            // Arrange
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var layout = new LayoutSqlRepository();
            PrivateObject privateLayoutObject = new PrivateObject(layout);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layout, new SeatSqlRepository(), venueMock.Object);

            // Act
            buisness_layer.AddLayout(id, name, venueId, description);
            buisness_layer.AddLayout(id + 1, name + "aaa", venueId, description);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateLayout(id + 1, name, venueId, description)).Should().Throw<Exception>().WithMessage("There is already a layout with such venue");
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(50)]
        public void RemoveLayout_WhenDataIsValid_ShouldReturnNull(int id)
        {
            // Arrange
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var layout = new LayoutSqlRepository();
            PrivateObject privateLayoutObject = new PrivateObject(layout);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layout, new SeatSqlRepository(), venueMock.Object);
            buisness_layer.AddLayout(id, "aaa", 100, "Some descr");

            // Act
            buisness_layer.DeleteLayout(id);

            // Assert
            var layouts = (List<Layout>)privateLayoutObject.GetField("_layouts");
            layouts.Should().NotContain(new Layout(id, "aaa", 100, "Some descr"));
        }
    }
}
