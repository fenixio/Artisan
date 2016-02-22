using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artisan.Tools.CacheManager
{
    public class SlidingExpirationPolicy : ExpirationPolicyBase
    {
        public TimeSpan ExpirateAfter { get; set; }

        public SlidingExpirationPolicy(TimeSpan expirateAfter)
        {
            ExpirateAfter = expirateAfter;
        }

        public SlidingExpirationPolicy(int seconds) : this( new TimeSpan(0,0,0,seconds) )
        {

        }

        public override void Store<TKey, TValue>(CacheItem<TKey, TValue> item)
        {
            item.ExpirationDate = item.CreationDate.Add(ExpirateAfter);
        }

        public override void Hit<TKey, TValue>(CacheItem<TKey, TValue> item)
        {
            item.ExpirationDate = DateTime.Now.Add(ExpirateAfter);
        }

    }
}
