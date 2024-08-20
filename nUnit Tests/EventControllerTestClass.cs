using AutoMapper;
using Book_Events.Controllers;
using Book_Events.Domain.DTOS;
using Book_Events.Domain.Interfaces;
using Book_Events.Infrastructure.Context;
using Book_Events.Infrastructure.Context.Entities;
using Book_Events.Infrastructure.Interfaces;
using Book_Events.Plugins.Logging;
using Book_Events.ViewModels;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace nUnit_Tests
{
    [TestFixture]
    public class EventControllerTestClass
    {
        private Mock<IEventService> _eventServiceMock;
        private Mock<UserManager<AppUser>> _userManagerMock;
        private Mock<IEventRepository> _repositoryMock;
        private Mock<ILog> _Ilog;

        [SetUp]
        public void SetUp()
        {
            _eventServiceMock = new Mock<IEventService>();
            _userManagerMock = MockUserManager<AppUser>();
            _repositoryMock = new Mock<IEventRepository>();
            _Ilog = new Mock<ILog>();
        }

        [Test]
        public async Task TestGetUserEvents()
        {
            var user = new AppUser { Email = "rob@gmail.com" };
            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _eventServiceMock.Setup(es => es.GetUserEventsAsync(It.IsAny<string>())).ReturnsAsync(new List<BookEventDTO>());

            var controller = new EventController(_eventServiceMock.Object,_userManagerMock.Object, _Ilog.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, "rob@gmail.com") }, "mock")) }
            };

            var result = await controller.GetUserEvents() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task HomePageTest()
        {
            var controller = new EventController( _eventServiceMock.Object, _userManagerMock.Object, _Ilog.Object);

            var actionResult = await controller.HomePage();

            Assert.That(actionResult, Is.InstanceOf<ViewResult>(), "Expected a ViewResult");
            var viewResult = actionResult as ViewResult;
            Assert.That(viewResult.ViewName, Is.EqualTo("HomePage"), "Expected ViewName to be 'HomePage'");
            Assert.That(viewResult.Model, Is.InstanceOf<HomePageViewModel>(), "Expected model type to be HomePageViewModel");
            Assert.That(actionResult, Is.Not.Null);
        }

        [Test]
        public void Create_ActionExecutes_ReturnsViewForCreate()
        {
            var controller = new EventController(_eventServiceMock.Object, _userManagerMock.Object, _Ilog.Object);

            var result = controller.Create();

            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task EventsInvitedToo_ActionExecutes()
        {
            var controller = new EventController( _eventServiceMock.Object, _userManagerMock.Object, _Ilog.Object);

            var result = await controller.EventsInvitedToo();

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }


        private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
