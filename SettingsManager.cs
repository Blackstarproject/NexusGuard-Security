using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FuturisticAntivirus
{
    /// <summary>
    /// Manages saving and loading of application settings, including the encrypted API key.
    /// </summary>
    public static class SettingsManager
    {
        private static readonly string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NexusGuard", "settings.dat");
        private static readonly byte[] Entropy = { 1, 2, 3, 4, 5, 6, 7, 8 }; // Salt for encryption

        public static void SaveApiKey(string apiKey)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
                byte[] encryptedData = ProtectedData.Protect(Encoding.UTF8.GetBytes(apiKey), Entropy, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(SettingsPath, encryptedData);
            }
            catch (Exception) { /* Handle or log exception if needed */ }
        }

        public static string LoadApiKey()
        {
            try
            {
                if (!File.Exists(SettingsPath)) return string.Empty;

                byte[] encryptedData = File.ReadAllBytes(SettingsPath);
                byte[] decryptedData = ProtectedData.Unprotect(encryptedData, Entropy, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}