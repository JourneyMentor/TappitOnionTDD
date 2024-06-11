using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Infrastructure.Redis;

namespace User.Infrastructure.Tests
{
    [TestFixture]
    public class RedisCacheServiceTests
    {
        private Mock<IConnectionMultiplexer> _mockConnectionMultiplexer;
        private RedisCacheService _cacheService;
        private IDatabase _database;

        [SetUp]
        public void Setup()
        {
            _mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            _database = Mock.Of<IDatabase>();
            _mockConnectionMultiplexer.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_database);
            _cacheService = new RedisCacheService(_mockConnectionMultiplexer.Object);
        }

        [Test]
        public async Task GetAsync_ReturnsDeserializedObject_WhenKeyExists()
        {
            var testObject = new { Name = "Test" };
            var serializedObject = JsonConvert.SerializeObject(testObject);
            Mock.Get(_database).Setup(db => db.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(serializedObject);

            var result = await _cacheService.GetAsync<dynamic>("test_key");

            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Name);
        }

        [Test]
        public async Task SetAsync_SerializesAndSetsObject()
        {
            var testObject = new { Name = "Test" };
            var serializedObject = JsonConvert.SerializeObject(testObject);

            Mock.Get(_database).Setup(db => db.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);

            await _cacheService.SetAsync("test_key", testObject);

            Mock.Get(_database).Verify(db => db.StringSetAsync("test_key", serializedObject, null, When.Always, CommandFlags.None), Times.Once);
        }
    }
}