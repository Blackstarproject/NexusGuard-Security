using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FuturisticAntivirus
{
    public class QuarantinedFileInfo
    {
        public string OriginalFileName { get; set; }
        public string QuarantinedFileName { get; set; }
        public string OriginalPath { get; set; }
        public DateTime DateQuarantined { get; set; }
    }

    public static class QuarantineManager
    {
        private static readonly string QuarantinePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NexusGuard", "Quarantine");

        static QuarantineManager()
        {
            Directory.CreateDirectory(QuarantinePath);
            File.SetAttributes(QuarantinePath, File.GetAttributes(QuarantinePath) | FileAttributes.Hidden);
        }

        public static bool QuarantineFile(string filePath)
        {
            if (!File.Exists(filePath)) return false;

            try
            {
                string fileName = Path.GetFileName(filePath);
                string uniqueFileName = Guid.NewGuid().ToString() + "-" + fileName;
                string destinationFile = Path.Combine(QuarantinePath, uniqueFileName + ".quarantined");
                string manifestFile = Path.Combine(QuarantinePath, uniqueFileName + ".manifest");

                // Manifest content: OriginalPath|QuarantineDate
                string manifestContent = $"{filePath}|{DateTime.UtcNow:o}";

                File.Move(filePath, destinationFile);
                File.WriteAllText(manifestFile, manifestContent);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to quarantine file: {ex.Message}", "Quarantine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool RestoreFile(string quarantinedFileName)
        {
            string quarantinedFilePath = Path.Combine(QuarantinePath, quarantinedFileName + ".quarantined");
            string manifestFilePath = Path.Combine(QuarantinePath, quarantinedFileName + ".manifest");

            if (!File.Exists(quarantinedFilePath) || !File.Exists(manifestFilePath)) return false;

            try
            {
                var manifestContent = File.ReadAllText(manifestFilePath).Split('|');
                string originalPath = manifestContent[0];
                string originalDirectory = Path.GetDirectoryName(originalPath);

                Directory.CreateDirectory(originalDirectory);
                File.Move(quarantinedFilePath, originalPath);
                File.Delete(manifestFilePath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to restore file: {ex.Message}", "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool DeleteFile(string quarantinedFileName)
        {
            string quarantinedFilePath = Path.Combine(QuarantinePath, quarantinedFileName + ".quarantined");
            string manifestFilePath = Path.Combine(QuarantinePath, quarantinedFileName + ".manifest");

            if (!File.Exists(quarantinedFilePath) || !File.Exists(manifestFilePath)) return false;
            try
            {
                File.Delete(quarantinedFilePath);
                File.Delete(manifestFilePath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete file: {ex.Message}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static List<QuarantinedFileInfo> GetQuarantinedFiles()
        {
            var quarantinedFiles = new List<QuarantinedFileInfo>();
            var manifestFiles = Directory.GetFiles(QuarantinePath, "*.manifest");

            foreach (var manifestFile in manifestFiles)
            {
                try
                {
                    var manifestContent = File.ReadAllText(manifestFile).Split('|');
                    var fileInfo = new QuarantinedFileInfo
                    {
                        QuarantinedFileName = Path.GetFileNameWithoutExtension(manifestFile),
                        OriginalPath = manifestContent[0],
                        OriginalFileName = Path.GetFileName(manifestContent[0]),
                        DateQuarantined = DateTime.Parse(manifestContent[1]).ToLocalTime()
                    };
                    quarantinedFiles.Add(fileInfo);
                }
                catch { /* Skip malformed manifest */ }
            }
            return quarantinedFiles;
        }
    }
}