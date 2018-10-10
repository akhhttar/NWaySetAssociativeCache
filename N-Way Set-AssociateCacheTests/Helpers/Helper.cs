using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWaySetAssociativeCache.Entities;
using NWaySetAssociativeCache.Tests;
using NWaySetAssociativeCacheTests.Entities;

namespace NWaySetAssociativeCacheTests.Helpers
{
    public class Helper
    {
        public static int GetRandomeNumber(int maxValue)
        {
            return (new Random()).Next(1, maxValue);
        }

        public static CacheItem<CacheKey, CacheValue> BuildCacheItem(string strKey, string strValue)
        {
            var key = new CacheKey(strKey);
            var value = new CacheValue(strValue);
            return new CacheItem<CacheKey, CacheValue>(key, value);
        }
    }
}
