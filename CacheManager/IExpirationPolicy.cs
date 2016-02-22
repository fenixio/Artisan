using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artisan.Tools.CacheManager
{
    public interface IExpirationPolicy
    {
        void ClearAll();

        bool IsValid<TKey, TValue>(CacheItem<TKey, TValue> item);

        List<TKey> FindInvalidItems<TKey, TValue>(Dictionary<TKey, CacheItem<TKey, TValue>> items);

        void Store<TKey, TValue>(CacheItem<TKey, TValue> item);

        void Hit<TKey, TValue>(CacheItem<TKey, TValue> item);

        void Remove<TKey>(TKey key);

    }
}
