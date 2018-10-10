using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWaySetAssociativeCache.Entities;
using NWaySetAssociativeCache.ReplacementHandlers;
using NWaySetAssociativeCache.Tests;
using NWaySetAssociativeCacheTests.Entities;
using NWaySetAssociativeCacheTests.Helpers;

namespace NWaySetAssociativeCacheTests.Tests
{
    [TestClass]
    public class MRUReplacementHandlerTests :MRUReplacementHandler<CacheKey,CacheValue>
    {
        [TestMethod]
        public void GetKeyToReplace_Should_Return_Most_Recently_Used_Key()
        {
            // Arrange
            var itemsAdded = this.AddBulkCacheItems(10);
            var mostRecentlyUsedItem = itemsAdded[Helper.GetRandomeNumber(10) - 1];
            this.VisitCacheItems(new List<CacheItem<CacheKey,CacheValue>>() {mostRecentlyUsedItem});

            // Act
            var keyToPlace = GetKeyToReplace();

            // Assert
            keyToPlace.Should().Be(mostRecentlyUsedItem.Key);
        }

        [TestMethod]
        public void GetKeyToReplace_Should_Remove_Replaced_Key_From_LInkedList_And_Node_Lookup_Dictionary()
        {
            // Arrange
            var cacheItem = Helper.BuildCacheItem("some key", "some value");
            NotifyCacheItemAdded(cacheItem);
           
            // Act
            var keyToRplace = GetKeyToReplace();

            // Assert
            keyToRplace.Should().Be(cacheItem.Key);
            recentlyUsedKeyList.Contains(keyToRplace).Should().BeFalse();
            linkedListNodeLookup.ContainsKey(keyToRplace).Should().BeFalse();
        }
    }
}
