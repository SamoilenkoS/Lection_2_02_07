﻿using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Lection_2_DAL.CachingSystem
{
    public class RedisCachingRepository : ICacheRepository
    {
        private readonly IDatabase _redisDatabase;

        public RedisCachingRepository(IConnectionMultiplexer redis)
        {
            _redisDatabase = redis.GetDatabase(0);
        }

        public async Task<string> GetAsync(string key)
        {
            var result = await _redisDatabase.StringGetAsync(new RedisKey(key));

            return result.HasValue ? result : default(string);
        }

        public Task SaveAsync(string key, string value)
        {
            return _redisDatabase.StringSetAsync(
                new RedisKey(key),
                new RedisValue(value),
                TimeSpan.FromSeconds(150));
        }
    }
}
