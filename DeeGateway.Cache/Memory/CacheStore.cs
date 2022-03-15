using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace DeeGateway.Cache.Memory
{
    public class CacheStore: ICacheStore
    {
        MemoryCache _cache;

        //public delegate void PostEvictionDelegate(object key, object value, EvictionReason reason, object state);
        //PostEvictionDelegate postEvictionDelegate;
        public CacheStore()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }
        public CacheStore(long sizeLimit)
        {
            _cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = sizeLimit});
        }
        public object Get(string key)
        {
            object val = null;
            if (key != null && _cache.TryGetValue(key, out val))
            {
                return val;
            }
            else
            {
                return default;
            }
        }
        public object Set(string key, object value, int seconds)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions();
            if (seconds > 0)
                cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromSeconds(seconds));
            cacheEntryOptions.AddExpirationToken(new CancellationChangeToken(cancellationTokenSource.Token));
            //cacheEntryOptions.RegisterPostEvictionCallback(postEvictionDelegate);
            return _cache.Set(key, value, cacheEntryOptions);
        }

        public object GetOrCreate(string key, object value, int seconds)
        {
            return _cache.GetOrCreate(key, entry =>
                {
                    if (seconds > 0)
                    {
                        entry.SlidingExpiration = TimeSpan.FromSeconds(seconds);
                    }
                    entry.AddExpirationToken(new CancellationChangeToken(cancellationTokenSource.Token));
                    return value;
                }
             );
        }
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
        private  CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public void FlushAll()
        {
            //cancellationTokenSource.Cancel();
            var keys = GetCacheKeys();
            foreach(var key in keys)
            {
                Remove(key);
            }
        }
        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public List<string> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }
    }
}
 