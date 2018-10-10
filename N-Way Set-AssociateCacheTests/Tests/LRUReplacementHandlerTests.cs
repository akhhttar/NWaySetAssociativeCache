using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class LRUReplacementHandlerTests :LRUReplacementHandler<CacheKey,CacheValue>
    {
        [TestMethod]
        public void GetKeyToReplace_Should_Return_Least_Recently_Used_Key()
        {
            // Arrange
            var itemsAdded = this.AddBulkCacheItems(10);
            var leastRecentlyUsedItem = itemsAdded[Helper.GetRandomeNumber(10) - 1];
            this.VisitCacheItems(itemsAdded.Except(new List<CacheItem<CacheKey,CacheValue>>() {leastRecentlyUsedItem}));

            // Act
            var keyToReplace = GetKeyToReplace();

            // Assert
            keyToReplace.Should().Be(leastRecentlyUsedItem.Key);
        }

        [TestMethod]
        public void GetKeyToReplace_Should_Remove_Replaced_Key_From_LInkedList_And_Node_Lookup_Dictionary()
        {
            // Arrange
            var cacheItem = Helper.BuildCacheItem("some key", "some value");
            NotifyCacheItemAdded(cacheItem);
           
            // Act
            var keyToReplace = GetKeyToReplace();

            // Assert
            keyToReplace.Should().Be(cacheItem.Key);
            recentlyUsedKeyList.Contains(keyToReplace).Should().BeFalse();
            linkedListNodeLookup.ContainsKey(keyToReplace).Should().BeFalse();
        }
    }
}
