﻿using System;
using System.Configuration;
using Kore.Caching;
using StackExchange.Redis;

namespace Kore.Plugins.Caching.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        public const string CacheKeyPrefix = "Kore.Plugins.Caching.Redis.CacheKey";
        private readonly string connectionString;
        private ConnectionMultiplexer connection;

        public RedisCacheManager()
        {
            connectionString = ConfigurationManager.AppSettings["RedisCacheConnection"];
        }

        ~RedisCacheManager()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
        }

        protected IDatabase Database
        {
            get
            {
                if (connection == null)
                {
                    connection = GetConnection();
                }
                return connection.GetDatabase();
            }
        }

        public T Get<T>(string key)
        {
            string cacheKey = string.Concat(CacheKeyPrefix, ":Cache:", key);

            if (typeof(T) == typeof(string))
            {
                var result = Database.StringGet(cacheKey);
                if (result.HasValue)
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }
                return default(T);
            }

            return Database.HashGetAll(cacheKey).ConvertFromRedis<T>();
        }

        public void Set(string key, object data, int cacheTimeInMinutes)
        {
            string cacheKey = string.Concat(CacheKeyPrefix, ":Cache:", key);

            if (data is string)
            {
                Database.StringSet(cacheKey, (data as string), TimeSpan.FromMinutes(cacheTimeInMinutes));
            }
            else
            {
                Database.HashSet(cacheKey, data.ToHashEntries());
                Database.KeyExpire(cacheKey, TimeSpan.FromMinutes(cacheTimeInMinutes));
            }
        }

        public bool IsSet(string key)
        {
            string cacheKey = string.Concat(CacheKeyPrefix, ":Cache:", key);
            return Database.KeyExists(cacheKey);
        }

        public void Remove(string key)
        {
            string cacheKey = string.Concat(CacheKeyPrefix, ":Cache:", key);
            Database.KeyDelete(cacheKey);
        }

        public void RemoveByPattern(string pattern)
        {
            string cacheKeyPattern = string.Concat(CacheKeyPrefix, ":Cache:", pattern);
            Database.KeyDeleteByPattern(cacheKeyPattern);
        }

        public void Clear()
        {
            string cacheKeyPattern = string.Concat(CacheKeyPrefix, ":Cache:*");
            Database.KeyDeleteByPattern(cacheKeyPattern);

            //var endPoints = connection.GetEndPoints();

            //foreach (var endPoint in endPoints)
            //{
            //    connection.GetServer(endPoint).FlushDatabase();
            //}
        }

        protected ConnectionMultiplexer GetConnection()
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString", "Ensure 'RedisCacheConnection' is set in <appSettings>.");
            }

            return ConnectionMultiplexer.Connect(connectionString);
        }
    }
}