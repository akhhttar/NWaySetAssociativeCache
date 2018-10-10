using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWaySetAssociativeCache;
using NWaySetAssociativeCache.Interfaces;
using NWaySetAssociativeCache.Tests;
using NWaySetAssociativeCacheTests.Entities;
using FluentAssertions;
using NWaySetAssociativeCache.Cache;

namespace NWaySetAssociativeCacheTests.Tests
{
    [TestClass]
    public class DirectMapCacheTests
    {
        private IDirectMapCache<CacheKey, CacheValue> _cache;
        private const int _cacheSize = 10;
        private MockReplacementHandler mockReplacementHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockReplacementHandler = new MockReplacementHandler();
            _cache = new DirectMapCache<CacheKey, CacheValue>(_cacheSize, mockReplacementHandler);
        }

        [TestMethod()]
        public void Add_Should_Persist_Cache_Item()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");

            // Act
            _cache.Add(empNumber, empName);

            // Assert
            _cache.Count.Should().Be(1);
        }

        [TestMethod()]
        public void Add_Should_Notify_Replacement_Handler()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");

            // Act
            _cache.Add(empNumber, empName);

            // Assert
            _cache.Count.Should().Be(1);
            mockReplacementHandler.NotifyCacheItemAddedCallCount.Should().Be(1);
        }


        [TestMethod]
        public void Add_Should_Update_Existing_Item_If_Key_Is_Already_In_Cache()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");
            _cache.Add(empNumber, empName);
            empName = new CacheValue("Bill Gates");

            // Act
            _cache.Add(empNumber, empName);

            // Assert
            _cache.Count.Should().Be(1);
            _cache.Get(empNumber).Should().Be(empName);
        }

        [TestMethod()]
        public void Get_Should_Return_Correct_Cache_Item_When_Key_Exists()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");
            _cache.Add(empNumber, empName);

            // Act & Assert
            _cache.Get(empNumber).Should().Be(empName);
        }

        [TestMethod()]
        public void Get_Should_Return_Null_When_Key_Does_Not_Exist()
        {
            // Arrange
            var nonExistingKey = new CacheKey("001");

            // Act & Assert
            _cache.Get(nonExistingKey).Should().Be(null);
        }

        [TestMethod()]
        public void Get_Should_Notify_Replacement_Handler_When_Key_Exists()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");
            _cache.Add(empNumber, empName);

            // Act
            _cache.Get(empNumber);

            // Assert
            mockReplacementHandler.NotifyCacheItemRetrievedCallCount.Should().Be(1);
        }

        [TestMethod()]
        public void Get_Should_Not_Notify_Replacement_Handler_When_Key_Does_Not_Exist()
        {
            // Arrange
            var empNumber = new CacheKey("001");            

            // Act
            _cache.Get(empNumber);

            // Assert
            mockReplacementHandler.NotifyCacheItemRetrievedCallCount.Should().Be(0);
        }

        [TestMethod()]
        public void Clear_Should_Remove_All_Cache_Items()
        {
            // Arrange
            var empNumber1 = new CacheKey("001");
            var empName1 = new CacheValue("Steve Jobs");
            _cache.Add(empNumber1, empName1);

            var empNumber2 = new CacheKey("002");
            var empName2 = new CacheValue("Bill Gates");
            _cache.Add(empNumber2, empName2);
            _cache.Count.Should().Be(2);

            // Act 
            _cache.Clear();

            // Assert
            _cache.Count.Should().Be(0);
        }

        [TestMethod]
        public void Clear_Should_Notify_Replacement_Handler()
        {
            // Act
            _cache.Clear();

            // Assert
            mockReplacementHandler.NotifyCacheClearedCallCount.Should().Be(1);
        }

        [TestMethod()]
        public void Contains_Should_Return_True_If_Cache_Item_Exists()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");
            _cache.Add(empNumber, empName);

            // Act & Assert
            _cache.Contains(empNumber).Should().BeTrue();
        }

        [TestMethod()]
        public void Contains_Should_Return_False_If_Cache_Item_Does_Not_Exist()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");
            var empNumber2 = new CacheKey("002");
            _cache.Add(empNumber, empName);

            // Act & Assert
            _cache.Contains(empNumber2).Should().BeFalse();
        }
    }
}
