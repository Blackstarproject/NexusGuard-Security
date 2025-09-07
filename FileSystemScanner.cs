using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FuturisticAntivirus
{
    public class FileSystemScanner
    {
        public Task<List<string>> GetFilesInDirectoryAsync(string path)
        {
            return Task.Run(() =>
            {
                var files = new List<string>();
                GetFilesRecursively(path, files);
                return files;
            });
        }

        public Task<List<string>> GetAllSystemFilesAsync()
        {
            return Task.Run(() =>
            {
                var allFiles = new List<string>();
                foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed))
                {
                    GetFilesRecursively(drive.RootDirectory.FullName, allFiles);
                }
                return allFiles;
            });
        }

        private void GetFilesRecursively(string path, List<string> fileList)
        {
            try
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    fileList.Add(file);
                }

                foreach (string directory in Directory.GetDirectories(path))
                {
                    GetFilesRecursively(directory, fileList);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Debug.WriteLine($"Access denied to: {path}. Skipping.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred accessing {path}: {ex.Message}");
            }
        }
    }
}