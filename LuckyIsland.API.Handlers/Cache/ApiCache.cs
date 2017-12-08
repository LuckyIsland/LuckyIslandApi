using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace LuckyIsland.API.Handlers
{
    public class ApiCache
    {
        private MemoryCache Cache = MemoryCache.Default;

        private static ApiCache instance;
        public static ApiCache Instance
        {
            get
            {
                if (ApiCache.instance == null)
                    ApiCache.instance = new ApiCache();

                return ApiCache.instance;
            }
        }

        private ApiCache()
        { }

        public object GetCache(string key)
        {
            return this.Cache.Get(key.ToLower());
        }

        public void SetCache(string key, object value, CacheItemPolicy policy)
        {
            this.Cache.Set(key.ToLower(), value, policy);
        }
    }
}