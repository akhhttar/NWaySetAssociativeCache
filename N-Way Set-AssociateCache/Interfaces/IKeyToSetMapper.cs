using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWaySetAssociativeCache.Interfaces
{
    public interface IKeyToSetMapper<TKey>
    {
        int GetSetIndexForKey(TKey key, int numberOfSets);
    }
}
