using System.Linq;

namespace NWaySetAssociativeCache.ReplacementHandlers
{
    public class  LRUReplacementHandler<TKey,TValue> : RUReplacementHandlerBase<TKey,TValue>
    {
        /// <summary>
        /// Returns the least recently used key.
        /// </summary>
        /// <returns></returns>
        public override TKey GetKeyToReplace()
        {
            var result = recentlyUsedKeyList.Last.Value;
            recentlyUsedKeyList.RemoveLast();
            linkedListNodeLookup.Remove(result);

            return result;
        }
    }
}
