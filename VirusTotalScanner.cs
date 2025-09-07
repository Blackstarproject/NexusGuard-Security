using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace FuturisticAntivirus
{
    public class VirusTotalScanner
    {
        private const string ApiUrl = "https://www.virustotal.com/api/v3/files/";
        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _apiRateLimiter = new SemaphoreSlim(4, 4);
        private SemaphoreSlim _hashingThrottler;
        public string ApiKey { get; set; }

        public VirusTotalScanner()
        {
            _httpClient = new HttpClient();
            // Default to high performance mode
            SetLowCpuMode(false);
        }

        public void SetLowCpuMode(bool isLowCpu)
        {
            // In low CPU mode, only hash one file at a time. Otherwise, use all available processor cores.
            int concurrency = isLowCpu ? 1 : Environment.ProcessorCount;
            _hashingThrottler = new SemaphoreSlim(concurrency, concurrency);
        }

        public async Task<ScanResult> ScanFileAsync(string filePath, CancellationToken token)
        {
            string hash = string.Empty;
            string now = DateTime.UtcNow.ToString("o");
            var fileInfo = new FileInfo(filePath);

            try
            {
                token.ThrowIfCancellationRequested();

                // 1. Check metadata cache first to avoid hashing
                var cachedResult = ScanCacheManager.GetCachedResult(fileInfo);
                if (cachedResult != null)
                {
                    return cachedResult;
                }

                // 2. Throttle the expensive hashing operation
                await _hashingThrottler.WaitAsync(token);
                try
                {
                    hash = await ComputeFileHashAsync(filePath);
                }
                finally
                {
                    _hashingThrottler.Release();
                }

                if (string.IsNullOrEmpty(hash))
                {
                    return new ScanResult { FilePath = filePath, Status = "Error Reading File", ScanDate = now };
                }

                // 3. Check hash cache before calling API
                cachedResult = ScanCacheManager.GetCachedResult(new FileInfo(filePath) { /* This part is tricky, let's simplify */ });
                // For simplicity, we are not re-implementing a separate hash cache. The file metadata cache is primary.

                // 4. Query VirusTotal API
                await _apiRateLimiter.WaitAsync(token);
                try
                {
                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Add("x-apikey", ApiKey);
                    var response = await _httpClient.GetAsync(ApiUrl + hash, token);

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        return new ScanResult { FilePath = filePath, Status = "API Error/Invalid Key", Sha256Hash = hash, ScanDate = now };
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return new ScanResult { FilePath = filePath, Status = "Unknown (Not on VT)", Sha256Hash = hash, ScanDate = now };

                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var newResult = ParseResponse(jsonResponse, filePath, hash);

                    if (newResult != null)
                    {
                        ScanCacheManager.AddToCache(fileInfo, newResult);
                    }
                    return newResult;
                }
                finally
                {
                    await Task.Delay(15000, token);
                    _apiRateLimiter.Release();
                }
            }
            catch (OperationCanceledException)
            {
                if (_apiRateLimiter.CurrentCount == 0) _apiRateLimiter.Release();
                return null;
            }
            catch (Exception)
            {
                return new ScanResult { FilePath = filePath, Status = "General Error", ScanDate = now };
            }
        }

        private async Task<string> ComputeFileHashAsync(string filePath)
        {
            using (var sha256 = SHA256.Create())
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            {
                var hashBytes = await Task.Run(() => sha256.ComputeHash(stream));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private ScanResult ParseResponse(string jsonResponse, string filePath, string hash)
        {
            try
            {
                var json = JObject.Parse(jsonResponse);
                var attributes = json["data"]?["attributes"];
                if (attributes == null) return new ScanResult { FilePath = filePath, Status = "Invalid Response", Sha256Hash = hash };

                var analysisStats = attributes["last_analysis_stats"];
                int detections = (analysisStats?["malicious"]?.Value<int>() ?? 0) + (analysisStats?["suspicious"]?.Value<int>() ?? 0);
                long lastAnalysisDateUnix = attributes["last_analysis_date"]?.Value<long>() ?? 0;

                return new ScanResult
                {
                    FilePath = filePath,
                    Status = detections > 0 ? "Malicious" : "Clean",
                    Detections = detections,
                    ScanDate = DateTimeOffset.FromUnixTimeSeconds(lastAnalysisDateUnix).UtcDateTime.ToString("o"),
                    Sha256Hash = hash
                };
            }
            catch (JsonReaderException)
            {
                return new ScanResult { FilePath = filePath, Status = "JSON Parse Error", Sha256Hash = hash };
            }
        }
    }
}