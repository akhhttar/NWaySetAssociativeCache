using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NWaySetAssociativeCache.ReplacementHandlers;
using NWaySetAssociativeCacheTests.Entities;

namespace NWaySetAssociativeCache.Tests
{
    [TestClass()]
    public class SetAssociativeCacheTests
    {
        private SetAssociativeCache<CacheKey, CacheValue> cache;
        private int numberOfSets = 2;
        private int numberOfWays = 2;
        [TestInitialize()]
        public void Initialize()
        {            
            cache = new SetAssociativeCache<CacheKey, CacheValue>(numberOfWays, numberOfSets,
                () => new LRUReplacementHandler<CacheKey, CacheValue>());
        }     

        [TestMethod()]
        public void Add_Should_Persist_Cache_Item()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");

            // Act
            cache.Add(empNumber, empName);

            // Assert
            cache.Count.Should().Be(1);
        }

        [TestMethod]
        public void Add_Should_Update_Existing_Item_If_Key_Is_Already_In_Cache()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");            
            cache.Add(empNumber, empName);
            empName = new CacheValue("Bill Gates");

            // Act
            cache.Add(empNumber, empName);

            // Assert
            cache.Count.Should().Be(1);
            cache.Get(empNumber).Should().Be(empName);
        }

        [TestMethod]
        public void Add_Should_Replace_LRU_Item_When_Cache_Is_Full_And_LRU_Is_The_Replacement_Handler()
        {
            // Arrange
            int setIndex = 1;
            var cacheKeys = FillCacheSet(setIndex, cache);
            VisitCacheItems(cacheKeys.Except(new List<CacheKey>() { cacheKeys.First() }), cache);
            var leastRecentlyUsedKey = cacheKeys.First();

            // Act
            var empNumber = new CacheKey("001", setIndex);
            var empName = new CacheValue("Steve Jobs");
            cache.Add(empNumber, empName);

            // Assert
            cache.Contains(leastRecentlyUsedKey).Should().BeFalse();
            cache.Get(empNumber).Should().Be(empName);
        }

        [TestMethod]
        public void Add_Should_Replace_MRU_Item_When_Cache_Is_Full_And_MRU_Is_The_Replacement_Handler()
        {
            var mruCache = new SetAssociativeCache<CacheKey, CacheValue>(numberOfWays, numberOfSets,
                () => new MRUReplacementHandler<CacheKey,CacheValue>());

            // Arrange
            int setIndex = 1;
            var cacheKeys = FillCacheSet(setIndex, cache);
            VisitCacheItems(new List<CacheKey>() { cacheKeys.First() }, mruCache);
            var mostRecentlyUsedKey = cacheKeys.First();

            // Act
            var empNumber = new CacheKey("001", setIndex);
            var empName = new CacheValue("Steve Jobs");
            cache.Add(empNumber, empName);

            // Assert
            cache.Contains(mostRecentlyUsedKey).Should().BeFalse();
            cache.Get(empNumber).Should().Be(empName);
        }

        [TestMethod()]
        public void Add_Should_Update_Cache_Item_If_Already_Existed()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");

            // Act
            cache.Add(empNumber, empName);
            empName = new CacheValue("Bill Gates");
            cache.Add(empNumber, empName);

            // Assert
            cache.Count.Should().Be(1);
            cache[empNumber].Should().Be(empName);
        }

        [TestMethod()]
        public void Get_Should_Return_Correct_Cache_Item_When_Key_Exists()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");
            cache.Add(empNumber, empName);

            // Act & Assert
            cache.Get(empNumber).Should().Be(empName);
        }

        [TestMethod()]
        public void Get_Should_Return_Null_When_Key_Does_Not_Exist()
        {
            // Arrange
            var nonExistingKey = new CacheKey("001");            
            
            // Act & Assert
            cache.Get(nonExistingKey).Should().Be(null);
        }

        [TestMethod()]
        public void Clear_Should_Remove_All_Cache_Items()
        {
            // Arrange
            var empNumber1 = new CacheKey("001");
            var empName1 = new CacheValue("Steve Jobs");
            cache.Add(empNumber1, empName1);

            var empNumber2 = new CacheKey("002");
            var empName2 = new CacheValue("Bill Gates");
            cache.Add(empNumber2, empName2);
            cache.Count.Should().Be(2);

            // Act 
            cache.Clear();

            // Assert
            cache.Count.Should().Be(0);
        }

        [TestMethod()]
        public void Contains_Should_Return_True_If_Cache_Item_Exists()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");
            cache.Add(empNumber, empName);

            // Act & Assert
            cache.Contains(empNumber).Should().BeTrue();
        }

        [TestMethod()]
        public void Contains_Should_Return_False_If_Cache_Item_Does_Not_Exist()
        {
            // Arrange
            var empNumber = new CacheKey("001");
            var empName = new CacheValue("Steve Jobs");
            var empNumber2 = new CacheKey("002");
            cache.Add(empNumber, empName);

            // Act & Assert
            cache.Contains(empNumber2).Should().BeFalse();
        }
                
        private static void VisitCacheItems(IEnumerable<CacheKey> keysToVisit, SetAssociativeCache<CacheKey, CacheValue> cache)
        {
            foreach (var key in keysToVisit)
                cache.Get(key);
        }

        private static List<CacheKey> FillCacheSet(int setIndex, SetAssociativeCache<CacheKey, CacheValue> cache)
        {
            List<CacheKey> keyList = new List<CacheKey>();
            cache.Clear();

            for (int i = 0; i < cache.NumberOfWays; i++)
            {
                var empNumber = new CacheKey($"{i}", setIndex);
                var empName = new CacheValue($"Employee {i}");

                cache.Add(empNumber, empName);
                keyList.Add(empNumber);
;            }

            return keyList;
        }
    }
}