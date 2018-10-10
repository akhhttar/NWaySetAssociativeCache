using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NWaySetAssociativeCache.Entities;
using NWaySetAssociativeCache.Enums;
using NWaySetAssociativeCache.Interfaces;

namespace NWaySetAssociativeCache.Cache
{
    /// <summary>
    /// A simple key-value based cache class to store N number of cache items. Uses ReplacementHandler to replace existing item with new one when cache is full.    
    /// </summary>
    /// <typeparam name="TKey">Cache key type</typeparam>
    /// <typeparam name="TValue">Cache value type</typeparam>
    public class DirectMapCache<TKey, TValue> : IDirectMapCache<TKey,TValue>
    {
        private readonly IReplacementHandler<TKey,TValue> _replacementHandler;
        private readonly ConcurrentDictionary<TKey, TValue> _cacheDictionary;
        private object addLock = new object();
        private object notifyLock = new object();

        /// <summary>
        /// Creates cache with provided replacement handler.
        /// </summary>
        /// <param name="size">Maximum number of items cache can contain.</param>
        /// <param name="replacementHandler">ReplacementHandler to find cache item to remove when cache is full.</param>
        public DirectMapCache(int size, IReplacementHandler<TKey,TValue> replacementHandler)
        {
            _replacementHandler = replacementHandler;
            _cacheDictionary = new ConcurrentDictionary<TKey, TValue>();
            Size = size;
        }

        /// <summary>
        /// Number of items in cache.
        /// </summary>
        public int Count => _cacheDictionary.Count;

        /// <summary>
        /// Get or Set cache items.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get => Get(key);
            set => Add(key, value);
        }

        /// <summary>
        /// Maximum number of items that cache can contain.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Add or update cache item if already existed.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return True, if cache item is added or updated.</returns>
        /// <returns>Return False, if cache item was already in cache and not updated because another thread updated the same item at the same time (a rare case).</returns>
        public bool Add(TKey key, TValue value)
        {
            bool isAddedOrUpdated = true;

            if (_cacheDictionary.TryGetValue(key, out TValue oldValue))
            {
                if (_cacheDictionary.TryUpdate(key, value, oldValue))
                    RaiseEvent(CacheEventType.ItemUpdated, key, value);
                else
                    isAddedOrUpdated = false; // another thread updated the same key 
            }
            else
            {
                // Even though Dictionary's TryAdd() and TryRemove() methods are thread safe but we need to provide explicit locking. E.g.
                // 1) Cache.Count was Size - 1, thread one and two both evaluated Count >= Size statement as False at same time and Added new item in cache which can increase cache maximum size to NumberOfWays + 1.
                // 2) Cache.Count was equal to Size, thread one evaluated Count >= Size statement as True, Removed the Item, meanwhile Thread two added new item in the cache. This will also increase the cache maximum size to NumberOfWays + 1
                lock (addLock)
                {
                    if (_cacheDictionary.Count >= Size)
                    {
                        _cacheDictionary.TryRemove(_replacementHandler.GetKeyToReplace(), out TValue removedValue);
                    }

                    if (_cacheDictionary.TryAdd(key, value))
                        RaiseEvent(CacheEventType.ItemAdded, key, value);
                    else
                        isAddedOrUpdated = false; // another thread added the same key before entrying the lock
                }                
            }

            return isAddedOrUpdated;
        }

        /// <summary>
        /// Get cache item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Return <see cref="TValue"/>, if key exist in cache.</returns>
        /// <returns>Return null, if key doesn't exist in cache.</returns>
        public TValue Get(TKey key)
        {
            if (_cacheDictionary.TryGetValue(key, out TValue result))
                RaiseEvent(CacheEventType.ItemRetrieved, key, result);

            return result;
        }

        /// <summary>
        /// Clear all cache items.
        /// </summary>
        public void Clear()
        {
            _cacheDictionary.Clear();
            RaiseEvent(CacheEventType.CacheCleared);
        }

        /// <summary>
        /// Return True, if key exists in cache.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
            return _cacheDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Notify cache changes to IReplacementHandler in a synchronized way.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void RaiseEvent(CacheEventType eventType, TKey key = default(TKey), TValue value = default(TValue))
        {
            var cacheItem = new CacheItem<TKey,TValue>(key, value);
            // Ensure that notify calls to IReplacementHandler are synchronized/thread safe so that
            // we are not dependent on IReplacementHandler's thread safety implementation.
            lock (notifyLock)
            {
                switch (eventType)
                {
                    case CacheEventType.ItemAdded:
                        _replacementHandler.NotifyCacheItemAdded(cacheItem);
                        break;
                    case CacheEventType.ItemUpdated:
                        _replacementHandler.NotifyCacheItemUpdated(cacheItem);
                        break;
                    case CacheEventType.ItemRetrieved:
                        _replacementHandler.NotifyCacheItemRetrieved(cacheItem);
                        break;
                    case CacheEventType.CacheCleared:
                        _replacementHandler.NotifyCacheCleared();
                        break;
                    case CacheEventType.ItemRemoved:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
                }
            }            
        }
    }
}

