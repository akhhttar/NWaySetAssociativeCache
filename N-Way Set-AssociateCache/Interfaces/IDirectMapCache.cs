using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWaySetAssociativeCache.Interfaces
{
    public interface  IDirectMapCache<TKey, TValue> : ICache<TKey, TValue>
    {
        /// <summary>
        /// Maximum number of item cache can contain.
        /// </summary>
        int Size { get; }
    }
}
