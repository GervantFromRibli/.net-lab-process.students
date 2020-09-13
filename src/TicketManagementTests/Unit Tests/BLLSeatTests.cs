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
    // Class for testing methods that works with SeatSqlRepository in BLL
    [TestFixture]
    public class BLLSeatTests
    {
        [TestCase(300, 100, 1, 1)]
        [TestCase(120, 140, 2, 5)]
        [TestCase(300, 45, 5, 2)]
        public void AddSeat_WhenDataIsValid_ShouldBeEquivalent(int id, int areaId, int row, int number)
        {
            // Arrange
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Area(areaId, 100, "Some description", 1, 1));
            var seat = new SeatSqlRepository();
            PrivateObject privateSeatObject = new PrivateObject(seat);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), seat, new VenueSqlRepository());

            // Act
            buisness_layer.AddSeat(id, areaId, row, number);

            // Assert
            var seats = (List<Seat>)privateSeatObject.GetField("_seats");
            seats[0].Should().BeEquivalentTo(new Seat(id, areaId, row, number));
        }

        [TestCase(300, 100, -2, 1)]
        [TestCase(120, 140, 2, -5)]
        public void AddSeat_WhenDataIsNotValid_ShouldThrowException(int id, int areaId, int row, int number)
        {
            // Arrange
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Area(areaId, 100, "Some description", 1, 1));
            var seat = new SeatSqlRepository();
            PrivateObject privateSeatObject = new PrivateObject(seat);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), seat, new VenueSqlRepository());

            // Assert
            buisness_layer.Invoking(elem => elem.AddSeat(id, areaId, row, number)).Should().Throw<Exception>().WithMessage("Can`t add a seat");
        }

        [TestCase(300, 100, 1, 1)]
        [TestCase(120, 140, 2, 5)]
        [TestCase(300, 45, 5, 2)]
        public void AddSeat_WhenDataIsRepeatable_ShouldThrowException(int id, int areaId, int row, int number)
        {
            // Arrange
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Area(areaId, 100, "Some description", 1, 1));
            var seat = new SeatSqlRepository();
            PrivateObject privateSeatObject = new PrivateObject(seat);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), seat, new VenueSqlRepository());

            // Act
            buisness_layer.AddSeat(id, areaId, row, number);

            // Assert
            buisness_layer.Invoking(elem => elem.AddSeat(id + 1, areaId, row, number)).Should().Throw<Exception>().WithMessage("There is already a seat with this coords");
        }

        [TestCase(300, 100, 1, 1)]
        [TestCase(120, 140, 2, 5)]
        [TestCase(300, 45, 5, 2)]
        public void ReadSeat_WhenDataIsValid_ShouldReturnEventAreaThatEquivalent(int id, int areaId, int row, int number)
        {
            // Arrange
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Area(areaId, 100, "Some description", 1, 1));
            var seat = new SeatSqlRepository();
            PrivateObject privateSeatObject = new PrivateObject(seat);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), seat, new VenueSqlRepository());
            buisness_layer.AddSeat(id, areaId, row, number);

            // Act
            var seatCheck = buisness_layer.ReadSeat(id);

            // Assert
            seatCheck.Should().BeEquivalentTo(new Seat(id, areaId, row, number));
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(300)]
        public void ReadSeat_WhenDataIsNotValid_ShouldReturnNull(int id)
        {
            // Arrange
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Area(100, 100, "Some description", 1, 1));
            var seat = new SeatSqlRepository();
            PrivateObject privateSeatObject = new PrivateObject(seat);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), seat, new VenueSqlRepository());
            buisness_layer.AddSeat(id, 100, 1, 1);

            // Act
            var seatCheck = buisness_layer.ReadSeat(id + 1);

            // Assert
            seatCheck.Should().BeNull();
        }

        [TestCase(300, 100, 1, 1)]
        [TestCase(120, 140, 2, 5)]
        [TestCase(300, 45, 5, 2)]
        public void UpdateSeat_WhenDataIsValid_ShouldNotBeEquivalent(int id, int areaId, int row, int number)
        {
            // Arrange
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Area(areaId, 100, "Some description", 1, 1));
            var seat = new SeatSqlRepository();
            PrivateObject privateSeatObject = new PrivateObject(seat);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), seat, new VenueSqlRepository());
            buisness_layer.AddSeat(id, areaId, row, number);

            // Act
            buisness_layer.UpdateSeat(id, areaId, row + 2, number + 1);

            // Assert
            var seats = (List<Seat>)privateSeatObject.GetField("_seats");
            seats[0].Should().NotBeEquivalentTo(new Seat(id, areaId, row, number));
        }

        [TestCase(300, 100, 1, 1)]
        [TestCase(120, 140, 2, 5)]
        [TestCase(300, 45, 5, 2)]
        public void UpdateSeat_WhenDataIsNotValid_ShouldThrowException(int id, int areaId, int row, int number)
        {
            // Arrange
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Area(areaId, 100, "Some description", 1, 1));
            var seat = new SeatSqlRepository();
            PrivateObject privateSeatObject = new PrivateObject(seat);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), seat, new VenueSqlRepository());

            // Act
            buisness_layer.AddSeat(id, areaId, row, number);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateSeat(id, areaId, row - 3, number - 3)).Should().Throw<Exception>().WithMessage("Can`t update a seat");
        }

        [TestCase(300, 100, 1, 1)]
        [TestCase(120, 140, 2, 5)]
        [TestCase(300, 45, 5, 2)]
        public void UpdateSeat_WhenDataIsRepeatable_ShouldThrowException(int id, int areaId, int row, int number)
        {
            // Arrange
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Area(areaId, 100, "Some description", 1, 1));
            var seat = new SeatSqlRepository();
            PrivateObject privateSeatObject = new PrivateObject(seat);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), seat, new VenueSqlRepository());

            // Act
            buisness_layer.AddSeat(id, areaId, row, number);
            buisness_layer.AddSeat(id + 1, areaId, row + 2, number + 1);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateSeat(id + 1, areaId, row, number)).Should().Throw<Exception>().WithMessage("There is already a seat with this coords");
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(50)]
        public void RemoveSeat_WhenDataIsValid_ShouldReturnNull(int id)
        {
            // Arrange
            var areaMock = new Mock<AreaSqlRepository>();
            areaMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Area(100, 100, "Some description", 1, 1));
            var seat = new SeatSqlRepository();
            PrivateObject privateSeatObject = new PrivateObject(seat);
            BLL buisness_layer = new BLL(areaMock.Object, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), seat, new VenueSqlRepository());
            buisness_layer.AddSeat(id, 100, 1, 1);

            // Act
            buisness_layer.DeleteSeat(id);

            // Assert
            var seats = (List<Seat>)privateSeatObject.GetField("_seats");
            seats.Should().NotContain(new Seat(id, 100, 1, 1));
        }
    }
}
