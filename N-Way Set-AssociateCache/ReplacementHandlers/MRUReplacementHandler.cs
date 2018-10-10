namespace NWaySetAssociativeCache.ReplacementHandlers
{
    public class  MRUReplacementHandler<TKey,TValue> : RUReplacementHandlerBase<TKey,TValue>
    {
        /// <summary>
        /// Retruns the most recently used key.
        /// </summary>
        /// <returns></returns>
        public override TKey GetKeyToReplace()
        {
            var result = recentlyUsedKeyList.First.Value;
            recentlyUsedKeyList.RemoveFirst();
            linkedListNodeLookup.Remove(result);
            return result;
        }
    }
}
