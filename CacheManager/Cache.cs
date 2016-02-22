using Artisan.Tools.CacheManager.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Artisan.Tools.CacheManager
{
    /// <summary>
    /// Main Cache class.
    /// It's a singleton. Each combination of TKey, TValue generates new instances of the Cache
    /// </summary>
    /// <typeparam name="TKey">It is the cache key type</typeparam>
    /// <typeparam name="TValue">It is the cache type value</typeparam>
    public class Cache<TKey, TValue> where TKey : IComparable
    {
        /// <summary>
        /// 0.Singleton instance
        /// </summary>
        public static readonly Cache<TKey, TValue> Manager = new Cache<TKey, TValue>();

        #region private fields
        /// <summary>
        /// 3.Is the main repositry of cache items
        /// </summary>
        private Dictionary<TKey, CacheItem<TKey, TValue>> items { get; set; }

        /// <summary>
        /// 7.Dummy object to sychronize instances
        /// </summary>
        private object synchObject;
        
        
        /// <summary>
        /// Expression which retrieves the key value of a item 
        /// </summary>
        private Func<TValue, TKey> keyExpression       = null;


        /// <summary>
        /// 1.Policy used to expire a cache
        /// </summary>
        private IExpirationPolicy expirationPolicy = null;


        /// <summary>
        /// 4.Policy used to purge the least valuable items on cache
        /// </summary>
        private IPurgePolicy<TKey> purgePolicy = null;


        /// <summary>
        /// 3.Apply purge policy during a Store in asynchMode
        /// </summary>
        private bool purgeInAsynchMode = true;

        #endregion private fields

        /// <summary>
        /// private ctor, is intended to be used only by singleton instance
        /// </summary>
        private Cache()
        {
            this.synchObject = new object();
            items = new Dictionary<TKey, CacheItem<TKey, TValue>>();
        }


        #region public methods

        
        /// <summary>
        /// Set the expression which retrieves the key value of a item 
        /// </summary>
        /// <example>t => t.Key</example>
        /// <param name="keyExpression">New expression to be used or null if expression is not used</param>
        /// <returns>this instance</returns>
        public Cache<TKey, TValue> SetKeyExpression(Func<TValue, TKey> keyExpression)
        {
            Monitor.Enter(synchObject);
            try
            {
                if (items.Count > 0)
                {
                    throw new CacheNotEmptyException();
                }
                this.keyExpression = keyExpression;
            }
            finally
            {
                Monitor.Exit(synchObject);
            }
            return this;
        }

        /// <summary>
        /// Set the policy used to expire items on cache based on time.
        /// </summary>
        /// <param name="expirationPolicy">Instance of expiration policy</param>
        /// <returns>this instance</returns>
        public Cache<TKey, TValue> SetExpirationPolicy(IExpirationPolicy expirationPolicy)
        {
            Monitor.Enter(synchObject);
            try
            {
                if (items.Count > 0)
                {
                    throw new CacheNotEmptyException();
                }
                this.expirationPolicy = expirationPolicy;
            }
            finally
            {
                Monitor.Exit(synchObject);
            }
            return this;
        }

        /// <summary>
        /// Set the policy used to purge items from cache based on usage. 
        /// </summary>
        /// <param name="purgePolicy">Instance of purge policy</param>
        /// <returns>this instance</returns>
        public Cache<TKey, TValue> SetPurgePolicy(IPurgePolicy<TKey> purgePolicy)
        {
            Monitor.Enter(synchObject);
            try
            {
                if (items.Count > 0)
                {
                    throw new CacheNotEmptyException();
                }
                this.purgePolicy = purgePolicy;
                //if(this.purgePolicy != null)
                //    ((PurgePolicyBase<TKey, TValue>)this.purgePolicy).SetCache(this);
            }
            finally
            {
                Monitor.Exit(synchObject);
            }
            return this;
        }

        /// <summary>
        /// If set to true ( default) it applies the policies asynchronouly.
        /// </summary>
        /// <param name="asynchMode">True or False</param>
        /// <returns>this instance</returns>
        public Cache<TKey, TValue> SetAsynchronousMode(bool asynchMode)
        {
            Monitor.Enter(synchObject);
            try
            {
                if (items.Count > 0)
                {
                    throw new CacheNotEmptyException();
                }
                this.purgeInAsynchMode = asynchMode;
            }
            finally
            {
                Monitor.Exit(synchObject);
            }
            return this;
        }

        /// <summary>
        /// 1.Returns teh current items count.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// 2.Retrieve or set a item value based on its internal key
        /// </summary>
        /// <param name="key">Key of item to be retrieved</param>
        /// <returns>Value fo item related to fhis key, null if item doesn't exist</returns>
        public TValue this[TKey key]
        {
            get {
                return Retrieve(key);
            }
            set {
                Store(key, value);
            }
        }

        /// <summary>
        /// Retrieve a item based on its key
        /// </summary>
        /// <param name="key">Key of item to be retrieved</param>
        /// <returns>Value fo item related to fhis key, null if item doesn't exist or it doesn't valid</returns>
        public TValue Retrieve(TKey key)
        {
            TValue temp;
            TryGetValue(key, out temp);
            return temp;
        }

        /// <summary>
        /// Try to retrieve a item based on its key
        /// </summary>
        /// <param name="key">Key of item to be retrieved</param>
        /// <param name="value">OUT parameter, receives the retrieved item or null if item doesn't exist or it doesn't valid</param>
        /// <returns>True if the item was found asn is valid, else false</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            bool exist;

            value = default(TValue);
            CacheItem<TKey, TValue> cacheItem;
            exist = items.TryGetValue(key, out cacheItem);
            if (exist)
            {
                if (IsValid(cacheItem))
                {
                    // If item exists and it is valid, return the item value and refresh policies
                    value = cacheItem.Value;
                    cacheItem.Hit();

                    if (expirationPolicy != null) expirationPolicy.Hit<TKey, TValue>(cacheItem);

                    if (purgePolicy != null)
                    {
                        Monitor.Enter(synchObject);
                        try
                        {
                            purgePolicy.Hit(cacheItem.Key);
                        }
                        finally
                        {
                            Monitor.Exit(synchObject);
                        }

                    }
                    // TODO: mark a hit on performance counters
                }
                else
                {
                    Remove(key);
                    // TODO: mark a miss on performance counters
                }
            }
            else
            {
                // TODO: mark a miss on performance counters
            }
            return exist;
        }

        
        /// <summary>
        /// Store a item in cache using the KeyExpression to define the key
        /// </summary>
        /// <param name="value">Value the item to be stored</param>
        public void Store(TValue value)
        {
            if (keyExpression != null)
            {
                TKey key = keyExpression(value);

                Store(key, value);
            }
            else
            {
                throw new CacheException("Cache is not well initialized. A key expression should be set in order to use Store(value)");
            }
        }

        /// <summary>
        /// Store a item in cache using a external key
        /// </summary>
        /// <param name="key">Key of item</param>
        /// <param name="value">Value of item</param>
        public void Store(TKey key, TValue value)
        {
            Store(key, value, InvalidationMode.PurgeAll);
        }

        public void Remove(TValue value)
        {
            if (keyExpression != null)
            {
                TKey key = keyExpression(value);

                Remove(key);
            }
            else
            {
                throw new CacheException("Cache is not well initialized. A key expression should be set in order to use Remove(value)");
            }
        }


        /// <summary>
        /// Remove a item from cache
        /// </summary>
        /// <param name="key">Key of item to be removed</param>
        public void Remove(TKey key)
        {
            Monitor.Enter(synchObject);
            try
            {
                CacheItem<TKey, TValue> item;
                if (items.TryGetValue(key, out item))
                {
                    if (purgePolicy != null)
                    {
                        purgePolicy.Remove( key);
                    }

                    items.Remove(key);
                }
            }
            finally
            {
                Monitor.Exit(synchObject);
            }
        }

        /// <summary>
        /// Clear all items on cache
        /// </summary>
        public void ClearAll()
        {
            Monitor.Enter(synchObject);
            try
            {
                if (expirationPolicy != null)
                {
                    expirationPolicy.ClearAll();
                }

                if (purgePolicy != null)
                {
                    purgePolicy.ClearAll();
                }

                items.Clear();
            }
            finally
            {
                Monitor.Exit(synchObject);
            }
        }

        /// <summary>
        /// Get all items in one single run
        /// </summary>
        /// <returns>List of value loaded</returns>
        public List<TValue> GetAll()
        {
            List<TValue> list = new List<TValue>();
            foreach (CacheItem<TKey, TValue> current in this.items.Values)
            {
                list.Add(current.Value);
            }
            return list;
        }

        /// <summary>
        /// Get all the keys in the list
        /// </summary>
        /// <returns></returns>
        public List<TKey> GetAllKeys()
        {
            List<TKey> list = new List<TKey>();
            foreach (CacheItem<TKey, TValue> current in this.items.Values)
            {
                list.Add(current.Key);
            }
            return list;
        }

        #endregion public methods

        #region private methods

        /// <summary>
        /// Check for item validity, only intended to be used on private. 
        /// We want avoid clients applications use this functions and 
        /// it need to lock the object to ensure the data doesn't change
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsValid(CacheItem<TKey, TValue> item)
        {
            bool isValid = true;
            if (expirationPolicy != null)
            {
                isValid = expirationPolicy.IsValid<TKey, TValue>(item);
            }
            return isValid;
        }


        /// <summary>
        /// Store a item in cache using a external key
        /// </summary>
        /// <param name="key">Key of item</param>
        /// <param name="value">Value of item</param>
        /// <param name="mode">Invalidation mode, it allow to impact all policies, purge only or none</param>
        private void Store(TKey key, TValue value, InvalidationMode mode)
        {
            Monitor.Enter(synchObject);
            try
            {
                CacheItem<TKey, TValue> cacheItem = new CacheItem<TKey, TValue>(key, value);
                items[key] = cacheItem;

                if (expirationPolicy != null)
                {
                    expirationPolicy.Store<TKey, TValue>(cacheItem);
                }

                if (purgePolicy != null)
                {
                    TKey toRemove;
                    if (purgePolicy.Store(cacheItem.Key, out toRemove))
                    {
                        Remove(key);
                    }
                }
            }
            finally
            {
                Monitor.Exit(synchObject);
            }
        }

       
        
        #endregion private methods

    }
}
