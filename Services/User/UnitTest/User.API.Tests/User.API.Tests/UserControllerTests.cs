using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.API.Controllers;
using User.Application.Interfaces.Services;

namespace User.API.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserQueryService> _mockUserQueryService;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserQueryService = new Mock<IUserQueryService>();
            _controller = new UserController(_mockUserQueryService.Object);
        }

        [Test]
        public async Task Get_ReturnsAllUsers()
        {
            var users = new List<User.Domain.Entities.User>
            {
                new User.Domain.Entities.User { /* Initialize properties */ },
                new User.Domain.Entities.User { /* Initialize properties */ }
            };
            _mockUserQueryService.Setup(s => s.GetAllAsync<User.Domain.Entities.User>()).ReturnsAsync(users);

            var result = await _controller.Get();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(users, okResult.Value);
        }
    }
}