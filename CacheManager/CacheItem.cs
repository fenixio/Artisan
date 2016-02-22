using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artisan.Tools.CacheManager
{

    /// <summary>
    /// It is a placeholder for values in cache.
    /// It persist usefull info togheter the value.
    /// </summary>
    /// <typeparam name="TKey">Type of key elements in cache</typeparam>
    /// <typeparam name="TValue">Type of value elements in cache</typeparam>
    public class CacheItem<TKey, TValue>
    {
        /// <summary>
        /// Date of creation of item
        /// </summary>
        internal DateTime CreationDate { get; private set; }

        /// <summary>
        /// Date of expiration of item
        /// </summary>
        internal DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Last access to this item
        /// </summary>
        internal DateTime LastAccessDate { get; private set; }

        /// <summary>
        /// Key of this item
        /// </summary>
        internal TKey Key { get; private set; }

        /// <summary>
        /// Value of this item
        /// </summary>
        internal TValue Value { get; private set; }


        /// <summary>
        /// Internal ctor, set default values
        /// </summary>
        internal CacheItem()
        {
            this.CreationDate   = DateTime.Now;
            this.LastAccessDate = DateTime.Now;
            this.ExpirationDate = DateTime.MaxValue;
        }

        /// <summary>
        /// Internal ctor, set key and value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        internal CacheItem(TKey key, TValue value)
            : this()
        {
            this.Key            = key;
            this.Value          = value;
        }

        /// <summary>
        /// It updates the internal counters
        /// </summary>
        internal void Hit()
        {
            LastAccessDate = DateTime.Now;
        }
    }
}
