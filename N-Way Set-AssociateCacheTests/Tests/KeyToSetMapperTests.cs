using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWaySetAssociativeCache.Cache;
using NWaySetAssociativeCache.Tests;

namespace NWaySetAssociativeCacheTests.Tests
{
    [TestClass]
    public class KeyToSetMapperTests
    {
        [TestMethod]
        public void GetSetIndexForKey_Should_Return_Index_GreaterOrEqualTo_0_And_LessThan_MaximumNumberOfSets()
        {
            var keyToSetMapper = new KeyHashCodeToSetMapper<CacheKey>();
            var maximumNumberOfSets = 100;
            for (int hashCode = -(maximumNumberOfSets * 2); hashCode <= maximumNumberOfSets * 2 ; hashCode++)
            {
                var index = keyToSetMapper.GetSetIndexForKey(new CacheKey($"key for {hashCode}", hashCode), maximumNumberOfSets);
                index.Should().BeInRange(0, maximumNumberOfSets - 1);
            }
        }
    }
}
