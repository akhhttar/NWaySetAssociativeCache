using System;
using NWaySetAssociativeCache.Interfaces;

namespace NWaySetAssociativeCache.Cache
{
    /// <summary>
    /// Default Cache Key to Set Mapper using HashCode.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class KeyHashCodeToSetMapper<TKey> : IKeyToSetMapper<TKey>
    {   
        /// <summary>
        /// Returns set index for the provided key.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="numberOfSets">Number of sets in N-Way Set-Associative Cache</param>
        /// <returns>Set Index for the provided key</returns>
        public int GetSetIndexForKey(TKey key, int numberOfSets)
        {
            // hash code can be negative so convert the index to absolute value
            return Math.Abs(key.GetHashCode() % numberOfSets);
        }
    }
}
