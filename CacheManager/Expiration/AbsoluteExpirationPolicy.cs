using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artisan.Tools.CacheManager
{
    public class AbsoluteExpirationPolicy : ExpirationPolicyBase
    {
        public int ExpirateOn { get; set; }

        public AbsoluteExpirationPolicy( int expirateOn) 
        {
            ExpirateOn = expirateOn;
        }

        public AbsoluteExpirationPolicy( SchedulePeriod period, int expirateOn)
        {
            switch(period)
            {
                case SchedulePeriod.Day:
                    ExpirateOn = expirateOn * 86400;
                    break;
                case SchedulePeriod.Hour:
                    ExpirateOn = expirateOn * 3600;
                    break;
                default:
                    ExpirateOn = expirateOn;
                    break;
            }
        }

        public override void Store<TKey, TValue>(CacheItem<TKey, TValue> item)
        {
            item.ExpirationDate = item.CreationDate.AddSeconds( ExpirateOn);
        }

    }
}
