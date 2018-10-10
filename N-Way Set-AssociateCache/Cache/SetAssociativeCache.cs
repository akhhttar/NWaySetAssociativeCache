using NWaySetAssociativeCache.Interfaces;
using System;
using System.Linq;
using NWaySetAssociativeCache.Cache;
using NWaySetAssociativeCache.ReplacementHandlers;

namespace NWaySetAssociativeCache
{
    /// <summary>
    /// In memory N-Way Set-Associative Cache.
    /// </summary>
    /// <typeparam name="TKey">Cache key type</typeparam>
    /// <typeparam name="TValue">Cache value type</typeparam>
    public class SetAssociativeCache<TKey, TValue> : ISetAssociateCache<TKey, TValue>
    {        
        private IDirectMapCache<TKey, TValue>[] _cacheSets;
        private IKeyToSetMapper<TKey> _keyToSetMapper;

        /// <summary>
        /// Create the cache with default ReplacementHandler and KeyToSetMapper
        /// </summary>
        /// <param name="numberOfWays">Number of ways in N-Way Set-Associative Cache</param>
        /// <param name="numberOfSets">Number of sets in N-Way Set-Associative Cache</param>
        public SetAssociativeCache(int numberOfWays, int numberOfSets)
            : this(numberOfWays, numberOfSets, () => new LRUReplacementHandler<TKey,TValue>(), new KeyHashCodeToSetMapper<TKey>())
        { }

        /// <summary>
        /// Create the cache with default KeyToSetMapper.
        /// </summary>
        /// <param name="numberOfWays">Number of ways in N-Way Set-Associative Cache</param>
        /// <param name="numberOfSets">Number of sets in N-Way Set-Associative Cache</param>
        /// <param name="replacementHandlerFactory">Factor method to create IReplacementHandler instance</param>
        public SetAssociativeCache(int numberOfWays, int numberOfSets, Func<IReplacementHandler<TKey,TValue>> replacementHandlerFactory) 
            : this(numberOfWays, numberOfSets, replacementHandlerFactory, new KeyHashCodeToSetMapper<TKey>())
        { }

        /// <summary>
        /// Create the cache with provided ReplacementHandler and KeyToSetMapper
        /// </summary>
        /// <param name="numberOfWays"></param>
        /// <param name="numberOfSets"></param>
        /// <param name="replacementHandlerFactory"></param>
        /// <param name="keyToSetMapper"></param>
        public SetAssociativeCache(int numberOfWays, int numberOfSets, Func<IReplacementHandler<TKey,TValue>> replacementHandlerFactory, IKeyToSetMapper<TKey> keyToSetMapper)
        {
            if (numberOfWays <= 0 )
                throw new Exception("Number of ways should be greater than 0.");

            if (numberOfWays <= 0)
                throw new Exception("Number of sets should be greater than 0.");            

            InitializeCache(numberOfWays, numberOfSets, replacementHandlerFactory, keyToSetMapper);
        }

        /// <summary>
        /// Number of ways in N-Way Set-Associative Cache
        /// </summary>
        public int NumberOfWays { get; private set; }

        /// <summary>
        /// Number of sets in N-Way Set-Associativr Cache
        /// </summary>
        public int NumberOfSets { get; private set; }

        /// <summary>
        /// Number of items in Cache
        /// </summary>
        public int Count => _cacheSets.Sum(s => s.Count);

        /// <summary>
        /// Get or Set Cache Item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get => Get(key);
            set => Add(key, value);
        }

        /// <summary>
        /// Initializes the cache.
        /// </summary>
        /// <param name="numberOfWays"></param>
        /// <param name="numberOfSets"></param>
        /// <param name="replacementHandlerFactory"></param>
        /// <param name="keyToSetMapper"></param>
        private void InitializeCache(int numberOfWays, int numberOfSets, Func<IReplacementHandler<TKey,TValue>> replacementHandlerFactory, IKeyToSetMapper<TKey> keyToSetMapper)
        {
            NumberOfWays = numberOfWays;
            NumberOfSets = numberOfSets;
            _keyToSetMapper = keyToSetMapper;

            _cacheSets = new IDirectMapCache<TKey, TValue>[numberOfSets];
            for (int i = 0; i < numberOfSets; i++)
            {
                _cacheSets[i] = new DirectMapCache<TKey, TValue>(numberOfWays, replacementHandlerFactory());
            }
        }

        /// <summary>
        /// Add or update cache item if already existed.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return True, if cache item is added or updated.</returns>
        /// <returns>Return False, if cache item was already in cache and not updated because another thread updated the same item at the same time (a rare case).</returns>
        public bool Add(TKey key, TValue value)
        {
            return _cacheSets[GetSetIndex(key)].Add(key, value);
        }

        /// <summary>
        /// Get cache item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Return <see cref="TValue"/>, if key exist in cache.</returns>
        /// <returns>Return null, if key doesn't exist in cache.</returns>
        public TValue Get(TKey key)
        {
            return _cacheSets[GetSetIndex(key)].Get(key);
        }

        /// <summary>
        /// Clear all cache items.
        /// </summary>
        public void Clear()
        {
            foreach (var set in _cacheSets)
            {
                set.Clear();
            }            
        }

        /// <summary>
        /// Return True, if key exists in cache.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
            return _cacheSets[GetSetIndex(key)].Contains(key);
        }

        /// <summary>
        /// Get the set index for cache key so that we can perform cache set/get operations in that set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int GetSetIndex(TKey key)
        {
            return _keyToSetMapper.GetSetIndexForKey(key, _cacheSets.Length);
        }
    }
}
