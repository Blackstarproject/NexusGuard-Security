using System;

namespace FuturisticAntivirus
{
    public class CacheItem
    {
        public long FileSize { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public ScanResult Result { get; set; }
    }
}