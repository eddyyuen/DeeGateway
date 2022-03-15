using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Cache.Memory
{
    public  class CacheManager : ICacheManager
    {
        private ConcurrentDictionary<string, ICacheStore> _cacheStoreList = new ConcurrentDictionary<string, ICacheStore>();

        private static CacheManager _cacheManager;
        public static CacheManager Instance
        {
            get
            {
                if (_cacheManager == null)
                {
                    _cacheManager = new CacheManager();
                }
                return _cacheManager;
            }
        }
        public bool CreateCacheStore(string name)
        {
            bool ret = false;
            if (!_cacheStoreList.ContainsKey(name))
            {
                ret = _cacheStoreList.TryAdd(name, new CacheStore());
            }
            return ret;
            
        }

        public bool CreateCacheStore(string name,long sizeLimit)
        {
            bool ret = false;
            if (!_cacheStoreList.ContainsKey(name))
            {
                ret = _cacheStoreList.TryAdd(name, new CacheStore(sizeLimit));
            }
            return ret;

        }

        public ICacheStore GetCacheStore(string name)
        {
            if (_cacheStoreList.TryGetValue(name, out ICacheStore Cache))
            {
                return Cache;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteCacheStore(string name)
        {
            if (_cacheStoreList.ContainsKey(name))
            {
               return _cacheStoreList.TryRemove(name,out ICacheStore cache);
            }
            else
            {
                return true;
            }

        }
    }
}
