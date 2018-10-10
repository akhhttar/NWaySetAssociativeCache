namespace NWaySetAssociativeCache.Interfaces
{
    public interface ICache<TKey, TValue>
    {
        /// <summary>
        /// Add or update cache item if already existed.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return True, if cache item is added or updated.</returns>
        /// <returns>Return False, if cache item was already in cache and not updated because another thread updated the same item at the same time (a rare case).</returns>
        bool Add(TKey key, TValue value);

        /// <summary>
        /// Get cache item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Return <see cref="TValue"/>, if key exist in cache.</returns>
        /// <returns>Return null, if key doesn't exist in cache.</returns>
        TValue Get(TKey key);

        /// <summary>
        /// Clear all cache items.
        /// </summary>
        void Clear();

        /// <summary>
        /// Return True, if key exists in cache.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(TKey key);


        /// <summary>
        /// Number of items in Cache
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get or Set Cache Item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TValue this[TKey key]
        {
            get;
            set;
        }
    }
}
