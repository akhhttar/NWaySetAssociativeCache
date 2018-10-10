using System.Collections.Generic;
using NWaySetAssociativeCache.Entities;
using NWaySetAssociativeCache.Interfaces;

namespace NWaySetAssociativeCache.ReplacementHandlers
{
    /// <summary>
    /// A base class for LRUReplacementHandler and MRUReplacementHandler to store cache keys in sorted order based on key usage.
    /// </summary>
    /// <typeparam name="TKey">Cache key type</typeparam>
    public abstract class RUReplacementHandlerBase<TKey, TValue> : IReplacementHandler<TKey, TValue>
    {
        /// <summary>
        /// Doubly LinkedList to store cache keys in sorted order based on key usage.
        /// </summary>
        protected LinkedList<TKey> recentlyUsedKeyList = new LinkedList<TKey>();

        // Using additional data structure for faster lookup (O(1)) of LinkedListNode to remove when CacheItem is retrieved. It will improve the runtime but will require additional space.
        // Not very happy with the approach. will think about possibilities to improve the space requirements, if time permits.
        protected Dictionary<TKey, LinkedListNode<TKey>> linkedListNodeLookup = new Dictionary<TKey, LinkedListNode<TKey>>();

        /// <summary>
        /// Notify that key is added in cache to let replacement handler re-arrange the indexes if needed.
        /// </summary>
        /// <param name="cacheItem">cache item added in cache</param>        
        public void NotifyCacheItemAdded(CacheItem<TKey, TValue> cacheItem)
        {
            linkedListNodeLookup.Add(cacheItem.Key, recentlyUsedKeyList.AddFirst(cacheItem.Key));
        }

        /// <summary>
        /// Notify that cache item is updated in cache to let replacement handler re-arrange the indexes if needed.
        /// </summary>        
        /// <param name="cacheItem">Cache item updated</param>        
        public void NotifyCacheItemUpdated(CacheItem<TKey,TValue> cacheItem)
        {
            CacheItemRetrievedOrUpdated(cacheItem);
        }

        /// <summary>
        /// Notify that cache item is retrieved from cache to let replacement handler re-arrange the indexes if needed.
        /// </summary>
        /// <param name="cacheItem">cache item retrieved from cache</param>        
        public void NotifyCacheItemRetrieved(CacheItem<TKey,TValue> cacheItem)
        {
            CacheItemRetrievedOrUpdated(cacheItem);
        }

        /// <summary>
        /// Notify that cache is cleared to let replacement handler clean its data as well.
        /// </summary>
        public void NotifyCacheCleared()
        {
            recentlyUsedKeyList.Clear();
            linkedListNodeLookup.Clear();
        }

        private void CacheItemRetrievedOrUpdated(CacheItem<TKey,TValue> cacheItem)
        {
                var nodeToDelete = linkedListNodeLookup[cacheItem.Key];
                linkedListNodeLookup.Remove(cacheItem.Key);
                recentlyUsedKeyList.Remove(nodeToDelete);

                linkedListNodeLookup.Add(cacheItem.Key, recentlyUsedKeyList.AddFirst(cacheItem.Key));
        }

        public abstract TKey GetKeyToReplace();
    }
}
