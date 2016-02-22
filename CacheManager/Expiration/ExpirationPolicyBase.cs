using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artisan.Tools.CacheManager
{
    public abstract class ExpirationPolicyBase : IExpirationPolicy
    {

        public void ClearAll()
        {

        }

        public virtual void Hit<TKey, TValue>(CacheItem<TKey, TValue> item)
        { 
            
        }

        public virtual bool IsValid<TKey, TValue>(CacheItem<TKey, TValue> item)
        {
            return item.ExpirationDate >= DateTime.Now;
        }

        public virtual List<TKey> FindInvalidItems<TKey, TValue>(Dictionary<TKey, CacheItem<TKey, TValue>> items)
        {
            List<TKey> toRemove = new List<TKey>();
            foreach (TKey key in items.Keys)
            {
                if (!IsValid<TKey, TValue>(items[key]))
                {
                    toRemove.Add(key);
                }
            }
            return toRemove;
        }

        public abstract void Store<TKey, TValue>(CacheItem<TKey, TValue> item);

        public virtual void Remove<TKey>(TKey key)
        {

        }
    }
}
