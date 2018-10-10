using NWaySetAssociativeCache.Entities;
using NWaySetAssociativeCache.Interfaces;
using NWaySetAssociativeCache.Tests;
using NWaySetAssociativeCacheTests.Entities;

namespace NWaySetAssociativeCacheTests.Tests
{
    public class MockReplacementHandler : IReplacementHandler<CacheKey,CacheValue>
    {
        public int NotifyCacheItemAddedCallCount  { get; private set; }
        public int NotifyCacheItemUpdatedCallCount { get; private set; }
        public int NotifyCacheItemRetrievedCallCount { get; private set; }
        public int NotifyCacheClearedCallCount { get; private set; }
        public int GetKeyToReplaceCallCount { get; private set; }

        public void NotifyCacheItemAdded(CacheItem<CacheKey,CacheValue> cacheItem)
        {
            NotifyCacheItemAddedCallCount++;
        }

        public void NotifyCacheItemUpdated(CacheItem<CacheKey, CacheValue> cacheItem)
        {
            NotifyCacheItemUpdatedCallCount++;
        }

        public void NotifyCacheItemRetrieved(CacheItem<CacheKey,CacheValue> cacheItem)
        {
            NotifyCacheItemRetrievedCallCount++;
        }

        public void NotifyCacheCleared()
        {
            NotifyCacheClearedCallCount++;
        }

        public CacheKey GetKeyToReplace()
        {
            GetKeyToReplaceCallCount++;
            return new CacheKey("fake key");
        }
    }
}