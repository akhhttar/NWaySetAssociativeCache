namespace NWaySetAssociativeCache.Interfaces
{
    public interface ISetAssociateCache<TKey, TValue> : ICache<TKey, TValue>
    {
        /// <summary>
        /// Number of ways in N-Way Set-Associative Cache
        /// </summary>
        int NumberOfWays { get; }

        /// <summary>
        /// Number of sets in N-Way Set-Associativr Cache
        /// </summary>
        int NumberOfSets { get; }
    }
}
