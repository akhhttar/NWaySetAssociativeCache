using NWaySetAssociativeCache.Entities;

namespace NWaySetAssociativeCache.Interfaces
{
    public interface IReplacementHandler<TKey,TValue>
    {
        void NotifyCacheItemAdded(CacheItem<TKey,TValue> cacheItem);
        void NotifyCacheItemUpdated(CacheItem<TKey, TValue> cacheItem);
        void NotifyCacheItemRetrieved(CacheItem<TKey, TValue> cacheItem);
        void NotifyCacheCleared();
        TKey GetKeyToReplace();
    }
}
