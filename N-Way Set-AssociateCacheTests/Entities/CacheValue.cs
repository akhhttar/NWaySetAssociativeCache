using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWaySetAssociativeCacheTests.Entities
{
    public class CacheValue
    {
        public CacheValue(string value)
        {
            this.Value = value;
        }

        public string Value { set; get; }

    }
}
