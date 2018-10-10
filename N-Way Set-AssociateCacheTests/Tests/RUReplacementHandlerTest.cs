using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NWaySetAssociativeCache.Entities;
using NWaySetAssociativeCache.ReplacementHandlers;
using NWaySetAssociativeCache.Tests;
using NWaySetAssociativeCacheTests.Entities;
using NWaySetAssociativeCacheTests.Helpers;

namespace NWaySetAssociativeCacheTests.Tests
{
    [TestClass]
    public class RUReplacementHandlerTests  : RUReplacementHandlerBase<CacheKey,CacheValue>
    {
        [TestMethod]
        public void NotifyCacheItemAdded_Should_Add_New_Key_AT_Top_Of_The_LinkedList()
        {
            // Arrange
            var cacheItem = Helper.BuildCacheItem("some key", "some value");

            // Act
            NotifyCacheItemAdded(cacheItem);

            // Assert
            recentlyUsedKeyList.First().Should().Be(cacheItem.Key);
        }

        [TestMethod]
        public void NotifyCacheItemAdded_Should_Add_New_Key_In_Node_Lookup_Dictionary()
        {
            // Arrange
            var cacheItem = Helper.BuildCacheItem("some key", "some value");

            // Act
            NotifyCacheItemAdded(cacheItem);

            // Assert
            linkedListNodeLookup.ContainsKey(cacheItem.Key).Should().BeTrue();
            linkedListNodeLookup[cacheItem.Key].Should().Be(recentlyUsedKeyList.First);
        }

        [TestMethod]
        public void NotifyCacheItemUpdated_Should_Move_Updated_Key_AT_Top_Of_The_LinkedList()
        {
            // Arrange
            var itemsAdded = this.AddBulkCacheItems(10);
            var itemToUpdate = itemsAdded[Helper.GetRandomeNumber(10) - 1];

            // Act
            NotifyCacheItemUpdated(itemToUpdate);

            // Assert
            recentlyUsedKeyList.First().Should().Be(itemToUpdate.Key);
            linkedListNodeLookup[itemToUpdate.Key].Should().Be(recentlyUsedKeyList.First);
        }

        [TestMethod]
        public void NotifyCacheItemRetrieved_Should_Move_Retrieved_Key_AT_Top_Of_The_LinkedList()
        {
            // Arrange
            var itemsAdded = this.AddBulkCacheItems(10);
            var itemToRetrieve = itemsAdded[Helper.GetRandomeNumber(10) - 1];

            // Act
            NotifyCacheItemRetrieved(itemToRetrieve);

            // Assert
            recentlyUsedKeyList.First().Should().Be(itemToRetrieve.Key);
            linkedListNodeLookup[itemToRetrieve.Key].Should().Be(recentlyUsedKeyList.First);
        }

        [TestMethod]
        public void NotifyCacheCleared_Should_Delete_All_Keys_From_LinkedList_And_Node_Lookup_Dictionary()
        {
            // Arrange
            this.AddBulkCacheItems(10);

            // Act
            NotifyCacheCleared();

            // Assert
            recentlyUsedKeyList.Count.Should().Be(0);
            linkedListNodeLookup.Count.Should().Be(0);
        }

        public override CacheKey GetKeyToReplace()
        {
            throw new NotImplementedException();
        }
    }
}
