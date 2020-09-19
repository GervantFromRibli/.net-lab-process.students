using System;
using System.Collections.Generic;
using System.Text;
using BuisnessLayer;
using DataAccessLayer;
using DomainEntities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;

namespace TicketManagementTests.Unit_Tests
{
    // Class for testing methods that works with AreaSqlRepository in BLL
    [TestFixture]
    public class BLLAreaTests
    {
        [TestCase(300, 200, "Some area", 10, 10)]
        [TestCase(120, 100, "Some another are", 0, 50)]
        [TestCase(300, 1, "Some third area", 4, 34)]
        public void AddArea_WhenDataIsValid_ShouldBeEquivalent(int id, int layoutId, string description, int coordX, int coordY)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(layoutId, "Some layout", 100, "Some description"));
            var area = new AreaSqlRepository();
            PrivateObject privateAreaObject = new PrivateObject(area);
            BLL buisness_layer = new BLL(area, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layoutMock.Object, new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddArea(id, layoutId, description, coordX, coordY);

            // Assert
            var areas = (List<Area>)privateAreaObject.GetField("_areas");
            areas[0].Should().BeEquivalentTo(new Area(id, layoutId, description, coordX, coordY));
        }

        [TestCase(300, 200, "Some area", 10, 10)]
        [TestCase(120, 100, "Some another are", 0, 50)]
        [TestCase(300, 1, "Some third area", 4, 34)]
        public void AddArea_WhenDataIsRepeatable_ShouldThrowException(int id, int layoutId, string description, int coordX, int coordY)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(layoutId, "Some layout", 100, "Some description"));
            var area = new AreaSqlRepository();
            PrivateObject privateAreaObject = new PrivateObject(area);
            BLL buisness_layer = new BLL(area, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layoutMock.Object, new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddArea(id, layoutId, description, coordX, coordY);

            // Assert
            buisness_layer.Invoking(elem => elem.AddArea(id, layoutId, description, coordX, coordY)).Should().Throw<Exception>().WithMessage("There is already a area with such layout and description");
        }

        [TestCase(300, 200, "Some area", -1, 10)]
        [TestCase(120, 100, "Some another are", 0, -7)]
        public void AddArea_WhenDataIsNotValid_ShouldThrowException(int id, int layoutId, string description, int coordX, int coordY)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(layoutId, "Some layout", 100, "Some description"));
            var area = new AreaSqlRepository();
            PrivateObject privateAreaObject = new PrivateObject(area);
            BLL buisness_layer = new BLL(area, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layoutMock.Object, new SeatSqlRepository(), new VenueSqlRepository());

            // Assert
            buisness_layer.Invoking(elem => elem.AddArea(id, layoutId, description, coordX, coordY)).Should().Throw<Exception>().WithMessage("Can`t add an area");
        }

        [TestCase(300, 200, "Some area", 10, 10)]
        [TestCase(120, 100, "Some another are", 0, 50)]
        [TestCase(300, 1, "Some third area", 4, 34)]
        public void ReadArea_WhenDataIsValid_ShouldReturnAreaThatEquivalent(int id, int layoutId, string description, int coordX, int coordY)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(layoutId, "Some layout", 100, "Some description"));
            var area = new AreaSqlRepository();
            BLL buisness_layer = new BLL(area, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layoutMock.Object, new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddArea(id, layoutId, description, coordX, coordY);

            // Act
            var areaCheck = buisness_layer.ReadArea(id);

            // Assert
            areaCheck.Should().BeEquivalentTo(new Area(id, layoutId, description, coordX, coordY));
        }

        [TestCase(300, 200, "Some area", 10, 10)]
        [TestCase(120, 100, "Some another are", 0, 50)]
        [TestCase(300, 1, "Some third area", 4, 34)]
        public void ReadArea_WhenDataIsNotValid_ShouldReturnNull(int id, int layoutId, string description, int coordX, int coordY)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(layoutId, "Some layout", 100, "Some description"));
            var area = new AreaSqlRepository();
            BLL buisness_layer = new BLL(area, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layoutMock.Object, new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddArea(id, layoutId, description, coordX, coordY);

            // Act
            var areaCheck = buisness_layer.ReadArea(id + 5);

            // Assert
            areaCheck.Should().BeNull();
        }

        [TestCase(300, 200, "Some area", 10, 10)]
        [TestCase(120, 100, "Some another are", 0, 50)]
        [TestCase(300, 1, "Some third area", 4, 34)]
        public void UpdateArea_WhenDataIsValid_ShouldNotBeEquivalent(int id, int layoutId, string description, int coordX, int coordY)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(layoutId, "Some layout", 100, "Some description"));
            var area = new AreaSqlRepository();
            PrivateObject privateAreaObject = new PrivateObject(area);
            BLL buisness_layer = new BLL(area, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layoutMock.Object, new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddArea(id, layoutId, description, coordX, coordY);

            // Act
            buisness_layer.UpdateArea(id, layoutId, "Not some area, but something strange", 0, 50);

            // Assert
            var areas = (List<Area>)privateAreaObject.GetField("_areas");
            areas[0].Should().NotBeEquivalentTo(new Area(id, layoutId, description, coordX, coordY));
        }

        [TestCase(300, 200, "Some area", 1, 10)]
        [TestCase(120, 100, "Some another are", 0, 7)]
        public void UpdateArea_WhenDataIsNotValid_ShouldThrowException(int id, int layoutId, string description, int coordX, int coordY)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(layoutId, "Some layout", 100, "Some description"));
            var area = new AreaSqlRepository();
            PrivateObject privateAreaObject = new PrivateObject(area);
            BLL buisness_layer = new BLL(area, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layoutMock.Object, new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddArea(id, layoutId, description, coordX, coordY);

            // Act
            buisness_layer.AddArea(id + 1, layoutId, description + "aaa", coordX, coordY);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateArea(id + 1, layoutId, description + "bbb", -1, -1)).Should().Throw<Exception>().WithMessage("Can`t update an area");
        }

        [TestCase(300, 200, "Some area", 10, 10)]
        [TestCase(120, 100, "Some another are", 0, 50)]
        [TestCase(300, 1, "Some third area", 4, 34)]
        public void UpdateArea_WhenDataIsRepeatable_ShouldThrowException(int id, int layoutId, string description, int coordX, int coordY)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(layoutId, "Some layout", 100, "Some description"));
            var area = new AreaSqlRepository();
            PrivateObject privateAreaObject = new PrivateObject(area);
            BLL buisness_layer = new BLL(area, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layoutMock.Object, new SeatSqlRepository(), new VenueSqlRepository());

            // Act
            buisness_layer.AddArea(id, layoutId, description, coordX, coordY);
            buisness_layer.AddArea(id + 1, layoutId, description + "aaa", coordX, coordY);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateArea(id + 1, layoutId, description + "aaa", coordX, coordY)).Should().Throw<Exception>().WithMessage("There is already a area with such layout and description");
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(50)]
        public void RemoveArea_WhenDataIsValid_ShouldReturnNull(int id)
        {
            // Arrange
            var layoutMock = new Mock<LayoutSqlRepository>();
            layoutMock.Setup(elem => elem.FindById(It.IsAny<int>())).Returns(new Layout(200, "Some layout", 100, "Some description"));
            var area = new AreaSqlRepository();
            PrivateObject privateAreaObject = new PrivateObject(area);
            BLL buisness_layer = new BLL(area, new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), layoutMock.Object, new SeatSqlRepository(), new VenueSqlRepository());
            buisness_layer.AddArea(id, 200, "Some area", 10, 10);

            // Act
            buisness_layer.DeleteArea(id);

            // Assert
            var areas = (List<Area>)privateAreaObject.GetField("_areas");
            areas.Should().NotContain(new Area(id, 200, "Some area", 10, 10));
        }
    }
}
