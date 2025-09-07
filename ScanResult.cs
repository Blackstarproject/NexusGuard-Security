using System.Collections.Generic;

namespace FuturisticAntivirus
{
    public class ScanResult
    {
        public string FilePath { get; set; }
        public string Status { get; set; }
        public int Detections { get; set; }
        public int TotalScanners { get; set; }
        public string ScanDate { get; set; }
        public string Sha256Hash { get; set; }
        public Dictionary<string, string> PositiveDetections { get; set; } = new Dictionary<string, string>();

        // New Property
        public bool IsFromCache { get; set; }
    }
}