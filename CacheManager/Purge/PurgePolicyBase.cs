
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artisan.Tools.CacheManager;

namespace Artisan.Tools.CacheManager.Purge
{
    public abstract class PurgePolicyBase<TKey> : IPurgePolicy<TKey>
        where TKey : IComparable
    {
        #region IPurgePolicy<TKey,TValue> Members

        public abstract void ClearAll();

        public abstract bool Store(TKey key, out TKey toRemove);

        public abstract void Remove(TKey key);

        public abstract void Hit(TKey key);

        #endregion
    }
}
