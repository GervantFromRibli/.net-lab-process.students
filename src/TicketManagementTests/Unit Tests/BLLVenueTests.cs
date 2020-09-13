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
    // Class for testing methods that works with VenueSqlRepository in BLL
    [TestFixture]
    public class BLLVenueTests
    {
        [TestCase(300, "First name", "First description", "First address", "")]
        [TestCase(120, "Second name", "Second description", "Second address", "123456789")]
        [TestCase(300, "Third name", "Third description", "Third address", "6689326")]
        public void AddVenue_WhenDataIsValid_ShouldBeEquivalent(int id, string name, string description, string address, string phone)
        {
            // Arrange
            var venue = new VenueSqlRepository();
            PrivateObject privateVenueObject = new PrivateObject(venue);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), venue);

            // Act
            buisness_layer.AddVenue(id, name, description, address, phone);

            // Assert
            var venues = (List<Venue>)privateVenueObject.GetField("_venues");
            venues[0].Should().BeEquivalentTo(new Venue(id, name, description, address, phone));
        }

        [TestCase(300, "First name aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "First description", "First address", "")]
        [TestCase(300, "Third name", "Third description", "Third address", "668932611111111111111111111111")]
        public void AddVenue_WhenDataIsNotValid_ShouldThrowException(int id, string name, string description, string address, string phone)
        {
            // Arrange
            var venue = new VenueSqlRepository();
            PrivateObject privateVenueObject = new PrivateObject(venue);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), venue);

            // Assert
            buisness_layer.Invoking(elem => elem.AddVenue(id, name, description, address, phone)).Should().Throw<Exception>().WithMessage("Can`t add a venue");
        }

        [TestCase(300, "First name", "First description", "First address", "")]
        [TestCase(120, "Second name", "Second description", "Second address", "123456789")]
        [TestCase(300, "Third name", "Third description", "Third address", "6689326")]
        public void AddVenue_WhenDataIsRepeatable_ShouldThrowException(int id, string name, string description, string address, string phone)
        {
            // Arrange
            var venue = new VenueSqlRepository();
            PrivateObject privateVenueObject = new PrivateObject(venue);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), venue);

            // Act
            buisness_layer.AddVenue(id, name, description, address, phone);

            // Assert
            buisness_layer.Invoking(elem => elem.AddVenue(id + 1, name, description, address, phone)).Should().Throw<Exception>().WithMessage("There is already a venue with such name");
        }

        [TestCase(300, "First name", "First description", "First address", "")]
        [TestCase(120, "Second name", "Second description", "Second address", "123456789")]
        [TestCase(300, "Third name", "Third description", "Third address", "6689326")]
        public void ReadVenue_WhenDataIsValid_ShouldReturnEventAreaThatEquivalent(int id, string name, string description, string address, string phone)
        {
            // Arrange
            var venue = new VenueSqlRepository();
            PrivateObject privateVenueObject = new PrivateObject(venue);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), venue);
            buisness_layer.AddVenue(id, name, description, address, phone);

            // Act
            var seatCheck = buisness_layer.ReadVenue(id);

            // Assert
            seatCheck.Should().BeEquivalentTo(new Venue(id, name, description, address, phone));
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(45)]
        public void ReadVenue_WhenDataIsNotValid_ShouldReturnNull(int id)
        {
            // Arrange
            var venue = new VenueSqlRepository();
            PrivateObject privateVenueObject = new PrivateObject(venue);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), venue);
            buisness_layer.AddVenue(id, "Name", "Descr", "Address", "");

            // Act
            var seatCheck = buisness_layer.ReadVenue(id + 1);

            // Assert
            seatCheck.Should().BeNull();
        }

        [TestCase(300, "First name", "First description", "First address", "")]
        [TestCase(120, "Second name", "Second description", "Second address", "123456789")]
        [TestCase(300, "Third name", "Third description", "Third address", "6689326")]
        public void UpdateVenue_WhenDataIsValid_ShouldNotBeEquivalent(int id, string name, string description, string address, string phone)
        {
            // Arrange
            var venue = new VenueSqlRepository();
            PrivateObject privateVenueObject = new PrivateObject(venue);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), venue);
            buisness_layer.AddVenue(id, name, description, address, phone);

            // Act
            buisness_layer.UpdateVenue(id, name + "aaa", description + "bbb", address + "ccc", "");

            // Assert
            var venues = (List<Venue>)privateVenueObject.GetField("_venues");
            venues[0].Should().NotBeEquivalentTo(new Venue(id, name, description, address, phone));
        }

        [TestCase(300, "First name", "First description", "First address", "")]
        [TestCase(120, "Second name", "Second description", "Second address", "123456789")]
        [TestCase(300, "Third name", "Third description", "Third address", "6689326")]
        public void UpdateVenue_WhenDataIsNotValid_ShouldThrowException(int id, string name, string description, string address, string phone)
        {
            // Arrange
            var venue = new VenueSqlRepository();
            PrivateObject privateVenueObject = new PrivateObject(venue);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), venue);

            // Act
            buisness_layer.AddVenue(id, name, description, address, phone);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateVenue(id, name + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", description, address, phone + "11111111111111111111111111111111111111111111111")).Should().Throw<Exception>().WithMessage("Can`t update a venue");
        }

        [TestCase(300, "First name", "First description", "First address", "")]
        [TestCase(120, "Second name", "Second description", "Second address", "123456789")]
        [TestCase(300, "Third name", "Third description", "Third address", "6689326")]
        public void UpdateVenue_WhenDataIsRepeatable_ShouldThrowException(int id, string name, string description, string address, string phone)
        {
            // Arrange
            var venue = new VenueSqlRepository();
            PrivateObject privateVenueObject = new PrivateObject(venue);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), venue);

            // Act
            buisness_layer.AddVenue(id, name, description, address, phone);
            buisness_layer.AddVenue(id + 1, name + "bbbb", description + "aaaa", address + "ccc", phone);

            // Assert
            buisness_layer.Invoking(elem => elem.UpdateVenue(id + 1, name, description, address, phone)).Should().Throw<Exception>().WithMessage("There is already a venue with such name");
        }

        [TestCase(300)]
        [TestCase(120)]
        [TestCase(45)]
        public void RemoveVenue_WhenDataIsValid_ShouldReturnNull(int id)
        {
            // Arrange
            var venue = new VenueSqlRepository();
            PrivateObject privateVenueObject = new PrivateObject(venue);
            BLL buisness_layer = new BLL(new AreaSqlRepository(), new EventAreaSqlRepository(), new EventSqlRepository(), new EventSeatSqlRepository(), new LayoutSqlRepository(), new SeatSqlRepository(), venue);
            buisness_layer.AddVenue(id, "name", "Descr", "adddresss", "");

            // Act
            buisness_layer.DeleteVenue(id);

            // Assert
            var venues = (List<Venue>)privateVenueObject.GetField("_venues");
            venues.Should().NotContain(new Venue(id, "name", "Descr", "adddresss", ""));
        }
    }
}
