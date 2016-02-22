using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artisan.Tools.CacheManager;

namespace Artisan.Tools.CacheManager.Purge
{
    public class LRUPurgePolicy<TKey> : PurgePolicyBase<TKey>
        where TKey : IComparable
    {
        private int maxSize = 5000;
        private LinkedList<TKey> lrus = null;

        public LRUPurgePolicy( ) : this(5000)
        {

        }

        public LRUPurgePolicy( int maxSize)
        {
            this.maxSize = maxSize;
            this.lrus     = new LinkedList<TKey>();
        }

        public override void ClearAll()
        {
            lrus.Clear();
        }

        public override bool Store(TKey key, out TKey toRemove )
        {
            toRemove = lrus.Last.Value;
            lrus.AddFirst(key);
            return (lrus.Count >= maxSize);
        }

        public override void Remove(TKey key)
        {
            lrus.Remove( key);
        }

        public override void Hit(TKey key)
        {
            lrus.Remove(key);
            lrus.AddFirst(key);
        }
    }
}
