using AutoMapper;
using Book_Events.Controllers;
using Book_Events.Domain.DTOS;
using Book_Events.Domain.Interfaces;
using Book_Events.Domain.Services;
using Book_Events.Infrastructure.Context;
using Book_Events.Infrastructure.Context.Entities;
using Book_Events.Infrastructure.Interfaces;
using Book_Events.Infrastructure.Unit_of_Work;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nUnit_Tests
{
    [TestFixture]
    internal class EventServicesTests
    {
        private Mock<IEventRepository> _eventRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private EventServices _eventServices;
        private Mock<ILogger<EventServices>> _loggerMock;
        private Mock<ICommentRepository> _commentRepositoryMock;
        private Mock<UnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void SetUp()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<EventServices>>();
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<UnitOfWork>();
            _eventServices = new EventServices(_mapperMock.Object,_eventRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Test]
        public async Task CreateEventServiceTest()
        {
            // Arrange
            var eventDto = new BookEventDTO { Title = "New Event", Date = DateOnly.FromDateTime(DateTime.Now), Location = "Test Location", StartTime = "13:00",Type = EventType.Public };
            var newEvent = new BookEvent { Title = "New Event", Date = DateOnly.FromDateTime(DateTime.Now), Location = "Test Location", StartTime = "13:00", Type = EventType.Public };

            _mapperMock.Setup(m => m.Map<BookEvent>(eventDto)).Returns(newEvent);
            _eventRepositoryMock.Setup(repo => repo.InsertAsync(newEvent)).ReturnsAsync(true);

            // Act
            var result = await _eventServices.CreateEventAsync(eventDto, "test@example.com");

            // Assert
            Assert.That(result, Is.True);
            _mapperMock.Verify(m => m.Map<BookEvent>(eventDto), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.InsertAsync(newEvent), Times.Once, "Insert failed");
        }
    }
}
