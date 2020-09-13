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
    // Class for testing methods that works with EventSqlRepository in BLL
    [TestFixture]
    public class BLLEventTests
    {
        [TestCase(300, "First name", "First description", 100)]
        [TestCase(120, "Second name", "Second description", 140)]
        [TestCase(300, "Third name", "Third description", 45)]
        public void AddEvent_WhenDataIsValid_ShouldBeEquivalent(int id, string name, string description, int layoutId)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(200, "Some layout", 100, "Some description"));
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.GetAll()).Returns(new List<Area>() { new Area(100, 200, "Some description", 1, 1) });
            var seatMock = new Mock<SeatSqlRepository>();
            seatMock.Setup(elem => elem.GetAll()).Returns(new List<Seat>() { new Seat(100, 100, 1, 1) });
            var @event = new EventSqlRepository();
            PrivateObject privateEventObject = new PrivateObject(@event);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), @event, new EventSeatSqlRepository(), layoutMock.Object, seatMock.Object, venueMock.Object);
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            // Act
            buisness_layer.AddEvent(id, name, description, layoutId, startDate.AddDays(10), endDate.AddDays(10));

            // Assert
            var events = (List<Event>)privateEventObject.GetField("_events");
            events[0].Should().BeEquivalentTo(new Event(id, name, description, layoutId, startDate.AddDays(10), endDate.AddDays(10)));
        }

        [TestCase(300, "First name", "First description", 100)]
        public void AddEvent_WhenDataIsNotValid_ShouldThrowException(int id, string name, string description, int layoutId)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(200, "Some layout", 100, "Some description"));
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.GetAll()).Returns(new List<Area>() { new Area(100, 200, "Some description", 1, 1) });
            var seatMock = new Mock<SeatSqlRepository>();
            seatMock.Setup(elem => elem.GetAll()).Returns(new List<Seat>() { new Seat(100, 100, 1, 1) });
            var @event = new EventSqlRepository();
            PrivateObject privateEventObject = new PrivateObject(@event);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), @event, new EventSeatSqlRepository(), layoutMock.Object, seatMock.Object, venueMock.Object);

            // Act
            DateTime startDate = DateTime.Now;
            startDate.AddDays(10);
            DateTime endDate = DateTime.Now;

            // Assert
            buisness_layer.Invoking(elem => elem.AddEvent(id, name, description, layoutId, startDate, endDate)).Should().Throw<Exception>().WithMessage("Can`t add an event");
        }

        [TestCase(300, "First name", "First description", 100)]
        [TestCase(120, "Second name", "Second description", 140)]
        [TestCase(300, "Third name", "Third description", 45)]
        public void AddEvent_WhenDataIsRepeatable_ShouldThrowException(int id, string name, string description, int layoutId)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(200, "Some layout", 100, "Some description"));
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.GetAll()).Returns(new List<Area>() { new Area(100, 200, "Some description", 1, 1) });
            var seatMock = new Mock<SeatSqlRepository>();
            seatMock.Setup(elem => elem.GetAll()).Returns(new List<Seat>() { new Seat(100, 100, 1, 1) });
            var @event = new EventSqlRepository();
            PrivateObject privateEventObject = new PrivateObject(@event);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), @event, new EventSeatSqlRepository(), layoutMock.Object, seatMock.Object, venueMock.Object);
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            // Act
            buisness_layer.AddEvent(id, name, description, layoutId, startDate.AddDays(10), endDate.AddDays(10));

            // Assert
            buisness_layer.Invoking(elem => elem.AddEvent(id, name, description, layoutId, startDate.AddDays(10), endDate.AddDays(10))).Should().Throw<Exception>().WithMessage("There is already an event");
        }

        [TestCase(300, "First name", "First description", 100)]
        [TestCase(120, "Second name", "Second description", 140)]
        [TestCase(300, "Third name", "Third description", 45)]
        public void ReadEvent_WhenDataIsValid_ShouldReturnEventAreaThatEquivalent(int id, string name, string description, int layoutId)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(200, "Some layout", 100, "Some description"));
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.GetAll()).Returns(new List<Area>() { new Area(100, 200, "Some description", 1, 1) });
            var seatMock = new Mock<SeatSqlRepository>();
            seatMock.Setup(elem => elem.GetAll()).Returns(new List<Seat>() { new Seat(100, 100, 1, 1) });
            var @event = new EventSqlRepository();
            PrivateObject privateEventObject = new PrivateObject(@event);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), @event, new EventSeatSqlRepository(), layoutMock.Object, seatMock.Object, venueMock.Object);
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            buisness_layer.AddEvent(id, name, description, layoutId, startDate.AddDays(10), endDate.AddDays(10));

            // Act
            var seatCheck = buisness_layer.ReadEvent(id);

            // Assert
            seatCheck.Should().BeEquivalentTo(new Event(id, name, description, layoutId, startDate.AddDays(10), endDate.AddDays(10)));
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(300)]
        public void ReadEvent_WhenDataIsNotValid_ShouldReturnNull(int id)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(200, "Some layout", 100, "Some description"));
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.GetAll()).Returns(new List<Area>() { new Area(100, 200, "Some description", 1, 1) });
            var seatMock = new Mock<SeatSqlRepository>();
            seatMock.Setup(elem => elem.GetAll()).Returns(new List<Seat>() { new Seat(100, 100, 1, 1) });
            var @event = new EventSqlRepository();
            PrivateObject privateEventObject = new PrivateObject(@event);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), @event, new EventSeatSqlRepository(), layoutMock.Object, seatMock.Object, venueMock.Object);
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            buisness_layer.AddEvent(id, "Some name", "Some description", 100, startDate.AddDays(10), endDate.AddDays(10));

            // Act
            var seatCheck = buisness_layer.ReadEvent(id + 1);

            // Assert
            seatCheck.Should().BeNull();
        }

        [TestCase(300, "First name", "First description", 100)]
        [TestCase(120, "Second name", "Second description", 140)]
        [TestCase(300, "Third name", "Third description", 45)]
        public void UpdateEvent_WhenDataIsValid_ShouldNotBeEquivalent(int id, string name, string description, int layoutId)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(200, "Some layout", 100, "Some description"));
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.GetAll()).Returns(new List<Area>() { new Area(100, 200, "Some description", 1, 1) });
            var seatMock = new Mock<SeatSqlRepository>();
            seatMock.Setup(elem => elem.GetAll()).Returns(new List<Seat>() { new Seat(100, 100, 1, 1) });
            var @event = new EventSqlRepository();
            PrivateObject privateEventObject = new PrivateObject(@event);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), @event, new EventSeatSqlRepository(), layoutMock.Object, seatMock.Object, venueMock.Object);
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            buisness_layer.AddEvent(id, name, description, layoutId, startDate.AddDays(10), endDate.AddDays(10));

            // Act
            buisness_layer.UpdateEvent(id, name + "aaa", description + "aaa", layoutId, startDate.AddDays(10), endDate.AddDays(20));

            // Assert
            var events = (List<Event>)privateEventObject.GetField("_events");
            events[0].Should().NotBeEquivalentTo(new Event(id, name, description, layoutId, startDate.AddDays(10), endDate.AddDays(10)));
        }

        [TestCase(300, "First name", "First description", 100)]
        [TestCase(120, "Second name", "Second description", 140)]
        [TestCase(300, "Third name", "Third description", 45)]
        public void UpdateEvent_WhenDataIsNotValid_ShouldThrowException(int id, string name, string description, int layoutId)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(200, "Some layout", 100, "Some description"));
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.GetAll()).Returns(new List<Area>() { new Area(100, 200, "Some description", 1, 1) });
            var seatMock = new Mock<SeatSqlRepository>();
            seatMock.Setup(elem => elem.GetAll()).Returns(new List<Seat>() { new Seat(100, 100, 1, 1) });
            var @event = new EventSqlRepository();
            PrivateObject privateEventObject = new PrivateObject(@event);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), @event, new EventSeatSqlRepository(), layoutMock.Object, seatMock.Object, venueMock.Object);
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            // Act
            buisness_layer.AddEvent(id, name, description, layoutId, startDate.AddDays(10), endDate.AddDays(10));

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateEvent(id, name, description, layoutId + 1, startDate.AddDays(100), endDate.AddDays(10))).Should().Throw<Exception>().WithMessage("Can`t update an event");
        }

        [TestCase(300, "First name", "First description", 100)]
        [TestCase(120, "Second name", "Second description", 140)]
        [TestCase(300, "Third name", "Third description", 45)]
        public void UpdateEvent_WhenDataIsRepeatable_ShouldThrowException(int id, string name, string description, int layoutId)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(200, "Some layout", 100, "Some description"));
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.GetAll()).Returns(new List<Area>() { new Area(100, 200, "Some description", 1, 1) });
            var seatMock = new Mock<SeatSqlRepository>();
            seatMock.Setup(elem => elem.GetAll()).Returns(new List<Seat>() { new Seat(100, 100, 1, 1) });
            var @event = new EventSqlRepository();
            PrivateObject privateEventObject = new PrivateObject(@event);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), @event, new EventSeatSqlRepository(), layoutMock.Object, seatMock.Object, venueMock.Object);
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            // Act
            buisness_layer.AddEvent(id, name, description, layoutId, startDate.AddDays(10), endDate.AddDays(10));
            buisness_layer.AddEvent(id + 1, name + "aaa", description, layoutId, startDate.AddDays(20), endDate.AddDays(20));

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateEvent(id + 1, name, description, layoutId, startDate.AddDays(5), endDate.AddDays(15))).Should().Throw<Exception>().WithMessage("There is already an event");
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(50)]
        public void RemoveEvent_WhenDataIsValid_ShouldReturnNull(int id)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(200, "Some layout", 100, "Some description"));
            var venueMock = new Mock<VenueSqlRepository>();
            venueMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Venue(100, "Some name", "Some description", "Address"));
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.GetAll()).Returns(new List<Area>() { new Area(100, 200, "Some description", 1, 1) });
            var seatMock = new Mock<SeatSqlRepository>();
            seatMock.Setup(elem => elem.GetAll()).Returns(new List<Seat>() { new Seat(100, 100, 1, 1) });
            var @event = new EventSqlRepository();
            PrivateObject privateEventObject = new PrivateObject(@event);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), @event, new EventSeatSqlRepository(), layoutMock.Object, seatMock.Object, venueMock.Object);
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            buisness_layer.AddEvent(id, "aaa", "Some descr", 200, startDate.AddDays(10), endDate.AddDays(10));

            // Act
            buisness_layer.DeleteEvent(id);

            // Assert
            var events = (List<Event>)privateEventObject.GetField("_events");
            events.Should().NotContain(new Event(id, "aaa", "Some descr", 200, startDate.AddDays(10), endDate.AddDays(10)));
        }
    }
}
