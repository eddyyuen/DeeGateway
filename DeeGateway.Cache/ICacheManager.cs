using System;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Cache
{
    public interface ICacheManager
    {
        ICacheStore GetCacheStore(string name);
        bool CreateCacheStore(string name);

        bool DeleteCacheStore(string name);

    }
}
