using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artisan.Tools.CacheManager
{
    public interface IPurgePolicy<TKey>
    {
        void ClearAll();

        bool Store(TKey key, out TKey toRemove);

        void Remove(TKey key);

        void Hit(TKey key);
    }
}
