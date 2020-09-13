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
    // Class for testing methods that works with EventAreaSqlRepository in BLL
    [TestFixture]
    public class BLLEventAreaTests
    {
        [TestCase(300, 200, "Some event area", 10, 10, 0)]
        [TestCase(120, 100, "Some another event area", 0, 50, 140)]
        [TestCase(300, 1, "Some third event area", 4, 34, 45)]
        public void AddEventArea_WhenDataIsValid_ShouldBeEquivalent(int id, int eventId, string description, int coordX, int coordY, decimal price)
        {
            // Arrange
            var eventMock = new Mock<EventSqlRepository>();
            eventMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Event(eventId, "Some event", "Some description", 100, DateTime.Now, DateTime.Now));
            var eventArea = new EventAreaSqlRepository();
            PrivateObject privateEventAreaObject = new PrivateObject(eventArea);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventArea, eventMock.Object, new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddEventArea(id, eventId, description, coordX, coordY, price);

            // Assert
            var eventAreas = (List<EventArea>)privateEventAreaObject.GetField("_eventAreas");
            eventAreas[0].Should().BeEquivalentTo(new EventArea(id, eventId, description, coordX, coordY, price));
        }

        [TestCase(300, 200, "Some event area", -7, 10, 0)]
        [TestCase(120, 100, "Some another event area", 0, -1, 140)]
        [TestCase(300, 1, "Some third event area", 4, 34, -10)]
        public void AddEventArea_WhenDataIsNotValid_ShouldThrowException(int id, int eventId, string description, int coordX, int coordY, decimal price)
        {
            // Arrange
            var eventMock = new Mock<EventSqlRepository>();
            eventMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Event(eventId, "Some event", "Some description", 100, DateTime.Now, DateTime.Now));
            var eventArea = new EventAreaSqlRepository();
            PrivateObject privateEventAreaObject = new PrivateObject(eventArea);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventArea, eventMock.Object, new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());

            // Assert
            buisness_layer.Invoking(elem => elem.AddEventArea(id, eventId, description, coordX, coordY, price)).Should().Throw<Exception>().WithMessage("Can`t add an event area");
        }

        [TestCase(300, 200, "Some event area", 10, 10, 0)]
        [TestCase(120, 100, "Some another event area", 0, 50, 140)]
        [TestCase(300, 1, "Some third event area", 4, 34, 45)]
        public void AddEventArea_WhenDataIsRepeatable_ShouldThrowException(int id, int eventId, string description, int coordX, int coordY, decimal price)
        {
            // Arrange
            var eventMock = new Mock<EventSqlRepository>();
            eventMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Event(eventId, "Some event", "Some description", 100, DateTime.Now, DateTime.Now));
            var eventArea = new EventAreaSqlRepository();
            PrivateObject privateEventAreaObject = new PrivateObject(eventArea);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventArea, eventMock.Object, new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddEventArea(id, eventId, description, coordX, coordY, price);

            // Assert
            buisness_layer.Invoking(elem => elem.AddEventArea(id, eventId, description, coordX, coordY, price)).Should().Throw<Exception>().WithMessage("There is already an event area with such coords");
        }

        [TestCase(300, 200, "Some event area", 10, 10, 0)]
        [TestCase(120, 100, "Some another event area", 0, 50, 140)]
        [TestCase(300, 1, "Some third event area", 4, 34, 45)]
        public void ReadEventArea_WhenDataIsValid_ShouldReturnEventAreaThatEquivalent(int id, int eventId, string description, int coordX, int coordY, decimal price)
        {
            // Arrange
            var eventMock = new Mock<EventSqlRepository>();
            eventMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Event(eventId, "Some event", "Some description", 100, DateTime.Now, DateTime.Now));
            var eventArea = new EventAreaSqlRepository();
            PrivateObject privateEventAreaObject = new PrivateObject(eventArea);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventArea, eventMock.Object, new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddEventArea(id, eventId, description, coordX, coordY, price);

            // Act
            var areaCheck = buisness_layer.ReadEventArea(id);

            // Assert
            areaCheck.Should().BeEquivalentTo(new EventArea(id, eventId, description, coordX, coordY, price));
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(300)]
        public void ReadEventArea_WhenDataIsNotValid_ShouldReturnNull(int id)
        {
            // Arrange
            var eventMock = new Mock<EventSqlRepository>();
            eventMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Event(0, "Some event", "Some description", 100, DateTime.Now, DateTime.Now));
            var eventArea = new EventAreaSqlRepository();
            PrivateObject privateEventAreaObject = new PrivateObject(eventArea);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventArea, eventMock.Object, new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddEventArea(id, 0, "", 0, 0, 0);

            // Act
            var areaCheck = buisness_layer.ReadEventArea(id + 1);

            // Assert
            areaCheck.Should().BeNull();
        }

        [TestCase(300, 200, "Some event area", 10, 10, 0)]
        [TestCase(120, 100, "Some another event area", 0, 50, 140)]
        [TestCase(300, 1, "Some third event area", 4, 34, 45)]
        public void UpdateEventArea_WhenDataIsValid_ShouldNotBeEquivalent(int id, int eventId, string description, int coordX, int coordY, decimal price)
        {
            // Arrange
            var eventMock = new Mock<EventSqlRepository>();
            eventMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Event(eventId, "Some event", "Some description", 100, DateTime.Now, DateTime.Now));
            var eventArea = new EventAreaSqlRepository();
            PrivateObject privateEventAreaObject = new PrivateObject(eventArea);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventArea, eventMock.Object, new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddEventArea(id, eventId, description, coordX, coordY, price);

            // Act
            buisness_layer.UpdateEventArea(id, eventId, "Not some event area, but something strange", 0, 60, 444);

            // Assert
            var eventAreas = (List<EventArea>)privateEventAreaObject.GetField("_eventAreas");
            eventAreas[0].Should().NotBeEquivalentTo(new EventArea(id, eventId, description, coordX, coordY, price));
        }

        [TestCase(300, 200, "Some event area", -7, 10, 0)]
        [TestCase(120, 100, "Some another event area", 0, -1, 140)]
        [TestCase(300, 1, "Some third event area", 4, 34, -478)]
        public void UpdateEventArea_WhenDataIsNotValid_ShouldThrowException(int id, int eventId, string description, int coordX, int coordY, decimal price)
        {
            // Arrange
            var eventMock = new Mock<EventSqlRepository>();
            eventMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Event(eventId, "Some event", "Some description", 100, DateTime.Now, DateTime.Now));
            var eventArea = new EventAreaSqlRepository();
            PrivateObject privateEventAreaObject = new PrivateObject(eventArea);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventArea, eventMock.Object, new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddEventArea(id, eventId, description, 1, 1, 1);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateEventArea(id, eventId, description, coordX, coordY, price)).Should().Throw<Exception>().WithMessage("Can`t update an event area");
        }

        [TestCase(300, 200, "Some event area", 1, 10, 0)]
        [TestCase(120, 100, "Some another event area", 0, 1, 140)]
        [TestCase(300, 1, "Some third event area", 4, 34, 478)]
        public void UpdateEventArea_WhenDataIsRepeatable_ShouldThrowException(int id, int eventId, string description, int coordX, int coordY, decimal price)
        {
            // Arrange
            var eventMock = new Mock<EventSqlRepository>();
            eventMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Event(eventId, "Some event", "Some description", 100, DateTime.Now, DateTime.Now));
            var eventArea = new EventAreaSqlRepository();
            PrivateObject privateEventAreaObject = new PrivateObject(eventArea);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventArea, eventMock.Object, new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddEventArea(id, eventId, description, coordX, coordY, price);
            buisness_layer.AddEventArea(id + 1, eventId, description, 2, 2, 1);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateEventArea(id + 1, eventId, description, coordX, coordY, price)).Should().Throw<Exception>().WithMessage("There is already an event area with such coords");
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(50)]
        public void RemoveEventArea_WhenDataIsValid_ShouldReturnNull(int id)
        {
            // Arrange
            var eventMock = new Mock<EventSqlRepository>();
            eventMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Event(200, "Some event", "Some description", 100, DateTime.Now, DateTime.Now));
            var eventArea = new EventAreaSqlRepository();
            PrivateObject privateEventAreaObject = new PrivateObject(eventArea);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventArea, eventMock.Object, new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddEventArea(id, 200, "Some area", 10, 10, 456);

            // Act
            buisness_layer.DeleteEventArea(id);

            // Assert
            var eventAreas = (List<EventArea>)privateEventAreaObject.GetField("_eventAreas");
            eventAreas.Should().NotContain(new EventArea(id, 200, "Some area", 10, 10, 456));
        }
    }
}
