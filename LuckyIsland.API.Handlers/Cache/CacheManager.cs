using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace LuckyIsland.API.Handlers
{
    public class CacheManager
    {
        private string Key;

        public CacheManager(string key)
        {
            this.Key = key;
        }

        public object GetCache()
        {
            return ApiCache.Instance.GetCache(this.Key);
        }

        public void SetCache(object value, int second)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(second)
            };
            ApiCache.Instance.SetCache(this.Key, value, policy);
        }
    }
}