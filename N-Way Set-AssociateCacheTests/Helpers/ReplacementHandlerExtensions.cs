using NWaySetAssociativeCache.Tests;
using System.Collections.Generic;
using NWaySetAssociativeCache.Entities;
using NWaySetAssociativeCache.Interfaces;
using NWaySetAssociativeCacheTests.Entities;

namespace NWaySetAssociativeCacheTests.Helpers
{
    public static class ReplacementHandlerExtensions
    {
        public static void VisitCacheItems(this IReplacementHandler<CacheKey,CacheValue> ReplacementHandler, IEnumerable<CacheItem<CacheKey,CacheValue>> itemsToVisit)
        {
            foreach (var item in itemsToVisit)
               ReplacementHandler.NotifyCacheItemRetrieved(item);
        }

        public static List<CacheItem<CacheKey,CacheValue>> AddBulkCacheItems(this IReplacementHandler<CacheKey,CacheValue> ReplacementHandler, int numberOfKeyToAdd)
        {
            var itemsAdded = new List<CacheItem<CacheKey, CacheValue>>();

            for (int i = 0; i < numberOfKeyToAdd; i++)
            {
                var cacheItem = Helper.BuildCacheItem($"key{i}", $"value{i}");
                ReplacementHandler.NotifyCacheItemAdded(cacheItem);
                itemsAdded.Add(cacheItem);
            }

            return itemsAdded;
        }
    }
}
