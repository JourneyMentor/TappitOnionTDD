using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;
using User.Application.Interfaces.RedisCache;
using User.Application.Interfaces.UnitOfWorks;
using User.Application.Services.Querires;
using User.Infrastructure.Redis;

namespace User.Application.Tests
{
    [TestFixture]
    public class UserQueryServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRedisCacheService> _mockRedisCacheService;
        private Mock<IMapper> _mockMapperService;
        private UserQueryService _queryService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRedisCacheService = new Mock<IRedisCacheService>();
            _mockMapperService = new Mock<IMapper>(); 
            _queryService = new UserQueryService(null , _mockUnitOfWork.Object, _mockRedisCacheService.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsDataFromCache_IfAvailable()
        {
            var cachedUsers = new List<User.Domain.Entities.User> { new User.Domain.Entities.User() };
            _mockRedisCacheService.Setup(x => x.GetAsync<IEnumerable<User.Domain.Entities.User>>(It.IsAny<string>())).ReturnsAsync(cachedUsers);

            var result = await _queryService.GetAllAsync<User.Domain.Entities.User>();

            Assert.AreEqual(cachedUsers, result);
            _mockUnitOfWork.Verify(u => u.GetReadRepository<User.Domain.Entities.User>()
                .GetAllAsync(
                    It.IsAny<Expression<Func<User.Domain.Entities.User, bool>>>(),
                    It.IsAny<Func<IQueryable<User.Domain.Entities.User>, IIncludableQueryable<User.Domain.Entities.User, object>>>(),
                    It.IsAny<Func<IQueryable<User.Domain.Entities.User>, IOrderedQueryable<User.Domain.Entities.User>>>(),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        public async Task GetAllAsync_RetrievesDataFromDatabase_IfCacheMiss()
        {
            var databaseUsers = new List<User.Domain.Entities.User> { new User.Domain.Entities.User() };
            _mockRedisCacheService.Setup(x => x.GetAsync<IEnumerable<User.Domain.Entities.User>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<User.Domain.Entities.User>)null);
            _mockUnitOfWork.Setup(u => u.GetReadRepository<User.Domain.Entities.User>()
      .GetAllAsync(
          It.IsAny<Expression<Func<User.Domain.Entities.User, bool>>>(),
          It.IsAny<Func<IQueryable<User.Domain.Entities.User>, IIncludableQueryable<User.Domain.Entities.User, object>>>(),
          It.IsAny<Func<IQueryable<User.Domain.Entities.User>, IOrderedQueryable<User.Domain.Entities.User>>>(),
          It.IsAny<bool>()))
      .ReturnsAsync(databaseUsers);


            var result = await _queryService.GetAllAsync<User.Domain.Entities.User>();

            Assert.AreEqual(databaseUsers, result);
            _mockRedisCacheService.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }
    }
}