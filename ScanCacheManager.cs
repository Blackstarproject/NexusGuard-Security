using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FuturisticAntivirus
{
    public static class ScanCacheManager
    {
        private static readonly string CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NexusGuard", "scan_cache_v2.json");
        private static Dictionary<string, CacheItem> _cache;

        static ScanCacheManager()
        {
            LoadCache();
        }

        private static void LoadCache()
        {
            try
            {
                if (File.Exists(CachePath))
                {
                    string json = File.ReadAllText(CachePath);
                    _cache = JsonConvert.DeserializeObject<Dictionary<string, CacheItem>>(json) ?? new Dictionary<string, CacheItem>();
                }
                else
                {
                    _cache = new Dictionary<string, CacheItem>();
                }
            }
            catch
            {
                _cache = new Dictionary<string, CacheItem>();
            }
        }

        private static void SaveCache()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CachePath));
                string json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
                File.WriteAllText(CachePath, json);
            }
            catch { /* Handle potential save errors */ }
        }

        public static ScanResult GetCachedResult(FileInfo fileInfo)
        {
            if (_cache.TryGetValue(fileInfo.FullName, out var item))
            {
                // Check if the file's size and modification date match the cached entry.
                if (item.FileSize == fileInfo.Length && item.LastModifiedUtc == fileInfo.LastWriteTimeUtc)
                {
                    item.Result.IsFromCache = true;
                    return item.Result;
                }
            }
            return null;
        }

        public static void AddToCache(FileInfo fileInfo, ScanResult result)
        {
            if (string.IsNullOrEmpty(fileInfo?.FullName) || result == null) return;

            var cacheItem = new CacheItem
            {
                FileSize = fileInfo.Length,
                LastModifiedUtc = fileInfo.LastWriteTimeUtc,
                Result = result
            };

            result.IsFromCache = false;
            _cache[fileInfo.FullName] = cacheItem;
            SaveCache();
        }
    }
}