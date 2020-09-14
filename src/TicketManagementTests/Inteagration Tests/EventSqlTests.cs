using System;
using System.Collections.Generic;
using BuisnessLayer;
using DataAccessLayer;
using DomainEntities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;

namespace TicketManagementTests
{
    // Class that tests the work of ADO.NET and Event repository
    [TestFixture]
    public class EventSqlTests
    {
        [TestCase(500, "Some event name", "Some description", 200)]
        [TestCase(501, "Other event name", "Second description", 200)]
        [TestCase(502, "Another event name", "Third description", 201)]
        public void AddEvent_ShouldAddEventToDB(int id, string name, string description, int layoutId)
        {
            // Arrange
            var eventRep = new EventSqlRepository(@"Data Source = .\;Initial Catalog = TicketManagementTest; Integrated Security = true");
            var eventElem = new Event(id, name, description, layoutId, DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));

            // Act
            eventRep.FillRepositoryWithSqlData();
            eventRep.Create(eventElem);

            // Assert
            eventRep.FindById(id).Should().BeEquivalentTo(eventElem);
        }

        [TestCase(500, "New event name", "New description", 201)]
        [TestCase(501, "Other new event name", "Some new description", 201)]
        [TestCase(502, "Another new event name", "Another new description", 200)]
        public void UpdateEvent_ShouldUpdateEventInDB(int id, string name, string description, int layoutId)
        {
            // Arrange
            var eventRep = new EventSqlRepository(@"Data Source = .\;Initial Catalog = TicketManagementTest; Integrated Security = true");
            var eventElem = new Event(id, name, description, layoutId, DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));

            // Act
            eventRep.FillRepositoryWithSqlData();
            eventRep.Update(eventElem);

            // Assert
            eventRep.FindById(id).Should().BeEquivalentTo(eventElem);
        }

        [TestCase(500)]
        [TestCase(501)]
        [TestCase(502)]
        public void DeleteEvent_ShouldUpdateEventInDB(int id)
        {
            // Arrange
            var eventRep = new EventSqlRepository(@"Data Source = .\;Initial Catalog = TicketManagementTest; Integrated Security = true");

            // Act
            eventRep.FillRepositoryWithSqlData();
            eventRep.Remove(id);

            // Assert
            eventRep.FindById(id).Should().BeNull();
        }
    }
}
