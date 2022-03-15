using System;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Cache
{
    public interface ICacheStore
    {
        
        object Get(string key);
        object Set(string key, object value, int seconds);
        object GetOrCreate(string key, object value, int seconds);
        void Remove(string key);
        void FlushAll();
        List<string> GetCacheKeys();
    }
}
