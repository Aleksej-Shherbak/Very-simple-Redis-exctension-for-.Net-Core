using System;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Checkaso.Tools
{
    /**
     * Simple Redis extension. It gives ability to save an object to Radis db. And you don't need to serialize the
     * to string or byte array.
     */
    public static class LehaRedisExtension
    {
        public static void SetObject(this IDistributedCache distributedCache, string key, object value, int minutes = 5)
        {
            var options = new DistributedCacheEntryOptions(); 
            
            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes)); 
            
            distributedCache.SetString(key, JsonConvert.SerializeObject(value), options);
        }

        public static T GetObject<T>(this IDistributedCache distributedCache, string key)
        {
            var value = distributedCache.GetString(key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}