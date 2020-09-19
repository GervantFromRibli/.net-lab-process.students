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
    // Class for testing methods that works with EventSeatSqlRepository in BLL
    [TestFixture]
    public class BLLEventSeatTests
    {
        [TestCase(300, 200, 10, 10, 0)]
        [TestCase(120, 100, 0, 50, 140)]
        [TestCase(300, 1, 4, 34, 45)]
        public void AddEventSeat_WhenDataIsValid_ShouldBeEquivalent(int id, int eventAreaId, int row, int number, int state)
        {
            // Arrange
            var eventAreaMock = new Mock<EventAreaSqlRepository>();
            eventAreaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new EventArea(eventAreaId, 100, "Some description", 100, 100, 100));
            var eventSeat = new EventSeatSqlRepository();
            PrivateObject privateEventSeatObject = new PrivateObject(eventSeat);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventAreaMock.Object, new EventSqlRepository(), eventSeat, new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddEventSeat(id, eventAreaId, row, number, state);

            // Assert
            var eventSeats = (List<EventSeat>)privateEventSeatObject.GetField("_eventSeats");
            eventSeats[0].Should().BeEquivalentTo(new EventSeat(id, eventAreaId, row, number, state));
        }

        [TestCase(300, 200, -7, 10, 0)]
        [TestCase(120, 100, 0, -1, 140)]
        public void AddEventSeat_WhenDataIsNotValid_ShouldThrowException(int id, int eventAreaId, int row, int number, int state)
        {
            // Arrange
            var eventAreaMock = new Mock<EventAreaSqlRepository>();
            eventAreaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new EventArea(eventAreaId, 100, "Some description", 100, 100, 100));
            var eventSeat = new EventSeatSqlRepository();
            PrivateObject privateEventSeatObject = new PrivateObject(eventSeat);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventAreaMock.Object, new EventSqlRepository(), eventSeat, new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());

            // Assert
            buisness_layer.Invoking(elem => elem.AddEventSeat(id, eventAreaId, row, number, state)).Should().Throw<Exception>().WithMessage("Can`t add an event seat");
        }

        [TestCase(300, 200, 10, 10, 0)]
        [TestCase(120, 100, 0, 50, 140)]
        [TestCase(300, 1, 4, 34, 45)]
        public void AddEventSeat_WhenDataIsRepeatable_ShouldThrowException(int id, int eventAreaId, int row, int number, int state)
        {
            // Arrange
            var eventAreaMock = new Mock<EventAreaSqlRepository>();
            eventAreaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new EventArea(eventAreaId, 100, "Some description", 100, 100, 100));
            var eventSeat = new EventSeatSqlRepository();
            PrivateObject privateEventSeatObject = new PrivateObject(eventSeat);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventAreaMock.Object, new EventSqlRepository(), eventSeat, new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddEventSeat(id, eventAreaId, row, number, state);

            // Assert
            buisness_layer.Invoking(elem => elem.AddEventSeat(id, eventAreaId, row, number, state)).Should().Throw<Exception>().WithMessage("There is already an event seat with such coords");
        }

        [TestCase(300, 200, 10, 10, 0)]
        [TestCase(120, 100, 0, 50, 140)]
        [TestCase(300, 1, 4, 34, 45)]
        public void ReadEventSeat_WhenDataIsValid_ShouldReturnEventAreaThatEquivalent(int id, int eventAreaId, int row, int number, int state)
        {
            // Arrange
            var eventAreaMock = new Mock<EventAreaSqlRepository>();
            eventAreaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new EventArea(eventAreaId, 100, "Some description", 100, 100, 100));
            var eventSeat = new EventSeatSqlRepository();
            PrivateObject privateEventSeatObject = new PrivateObject(eventSeat);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventAreaMock.Object, new EventSqlRepository(), eventSeat, new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddEventSeat(id, eventAreaId, row, number, state);

            // Act
            var seatCheck = buisness_layer.ReadEventSeat(id);

            // Assert
            seatCheck.Should().BeEquivalentTo(new EventSeat(id, eventAreaId, row, number, state));
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(300)]
        public void ReadEventSeat_WhenDataIsNotValid_ShouldReturnNull(int id)
        {
            // Arrange
            var eventAreaMock = new Mock<EventAreaSqlRepository>();
            eventAreaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new EventArea(0, 100, "Some description", 100, 100, 100));
            var eventSeat = new EventSeatSqlRepository();
            PrivateObject privateEventSeatObject = new PrivateObject(eventSeat);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventAreaMock.Object, new EventSqlRepository(), eventSeat, new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddEventSeat(id, 0, 0, 0, 0);

            // Act
            var seatCheck = buisness_layer.ReadEventSeat(id + 1);

            // Assert
            seatCheck.Should().BeNull();
        }

        [TestCase(300, 200, 10, 10, 0)]
        [TestCase(120, 100, 0, 50, 140)]
        [TestCase(300, 1, 4, 34, 45)]
        public void UpdateEventSeat_WhenDataIsValid_ShouldNotBeEquivalent(int id, int eventAreaId, int row, int number, int state)
        {
            // Arrange
            var eventAreaMock = new Mock<EventAreaSqlRepository>();
            eventAreaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new EventArea(eventAreaId, 100, "Some description", 100, 100, 100));
            var eventSeat = new EventSeatSqlRepository();
            PrivateObject privateEventSeatObject = new PrivateObject(eventSeat);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventAreaMock.Object, new EventSqlRepository(), eventSeat, new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddEventSeat(id, eventAreaId, row, number, state);

            // Act
            buisness_layer.UpdateEventSeat(id, eventAreaId, 0, 60, 444);

            // Assert
            var eventSeats = (List<EventSeat>)privateEventSeatObject.GetField("_eventSeats");
            eventSeats[0].Should().NotBeEquivalentTo(new EventSeat(id, eventAreaId, row, number, state));
        }

        [TestCase(300, 200, -4, 10, 0)]
        [TestCase(120, 100, 0, -50, 140)]
        public void UpdateEventSeat_WhenDataIsNotValid_ShouldThrowException(int id, int eventAreaId, int row, int number, int state)
        {
            // Arrange
            var eventAreaMock = new Mock<EventAreaSqlRepository>();
            eventAreaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new EventArea(eventAreaId, 100, "Some description", 100, 100, 100));
            var eventSeat = new EventSeatSqlRepository();
            PrivateObject privateEventSeatObject = new PrivateObject(eventSeat);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventAreaMock.Object, new EventSqlRepository(), eventSeat, new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddEventSeat(id, eventAreaId, 1, 1, 1);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateEventSeat(id, eventAreaId, row, number, state)).Should().Throw<Exception>().WithMessage("Can`t update an event seat");
        }

        [TestCase(300, 200, 10, 10, 0)]
        [TestCase(120, 100, 0, 50, 140)]
        [TestCase(300, 1, 4, 34, 45)]
        public void UpdateEventSeat_WhenDataIsRepeatable_ShouldThrowException(int id, int eventAreaId, int row, int number, int state)
        {
            // Arrange
            var eventAreaMock = new Mock<EventAreaSqlRepository>();
            eventAreaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new EventArea(eventAreaId, 100, "Some description", 100, 100, 100));
            var eventSeat = new EventSeatSqlRepository();
            PrivateObject privateEventSeatObject = new PrivateObject(eventSeat);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventAreaMock.Object, new EventSqlRepository(), eventSeat, new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddEventSeat(id, eventAreaId, row, number, state);
            buisness_layer.AddEventSeat(id + 1, eventAreaId, 2, 2, 1);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateEventSeat(id + 1, eventAreaId, row, number, state)).Should().Throw<Exception>().WithMessage("There is already an event seat with such coords");
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(50)]
        public void RemoveEventSeat_WhenDataIsValid_ShouldReturnNull(int id)
        {
            // Arrange
            var eventAreaMock = new Mock<EventAreaSqlRepository>();
            eventAreaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new EventArea(200, 100, "Some description", 100, 100, 100));
            var eventSeat = new EventSeatSqlRepository();
            PrivateObject privateEventSeatObject = new PrivateObject(eventSeat);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), eventAreaMock.Object, new EventSqlRepository(), eventSeat, new LayoutSqlRepository(), new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddEventSeat(id, 200, 10, 10, 456);

            // Act
            buisness_layer.DeleteEventSeat(id);

            // Assert
            var eventSeats = (List<EventSeat>)privateEventSeatObject.GetField("_eventSeats");
            eventSeats.Should().NotContain(new EventSeat(id, 200, 10, 10, 456));
        }
    }
}
