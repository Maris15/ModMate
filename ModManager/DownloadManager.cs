using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Net.Http;
using SharpCompress.Archives;

namespace ModManager
{
    public class DownloadManager
    {
        string _gtaPath;
        Dictionary<string, ModInfo> _mods;

        public event EventHandler<DownloadProgressEventArgs> DownloadProgressChanged;
        public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;

        public DownloadManager(string gtaPath, Dictionary<string, ModInfo> mods)
        {
            _gtaPath = gtaPath;
            _mods = mods;
        }

        public async Task DownloadAndInstallModAsync(string modName)
        {
            if (!_mods.ContainsKey(modName))
            {
                OnDownloadCompleted(modName, false, "Error: Mod Not Found!");
                return;
            }

            ModInfo mod = _mods[modName];

            // temp folder just for testing purpses, very important - Maris
            // it's gonna make a folder in %temp% when a user download zip for example
            // zip file will be downloaded & extracted, then it copies the extracted folder and moves its content to the gtav main directory path
            // then it cleans the temp folder

            string tempDir = Path.Combine(Path.GetTempPath(), "LSPDFRModManager");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            string downloadPath = Path.Combine(tempDir, $"{modName}.zip");
            string extractPath = Path.Combine(tempDir, modName);


            if (File.Exists(downloadPath))
            {
                try
                {
                    Directory.Delete(extractPath, true);
                }
                catch (Exception ex)
                {
                    OnDownloadCompleted(modName, false, $"Failed to clean temporary files: {ex.Message}");
                    return;
                }
            }

            Directory.CreateDirectory(extractPath);

            try
            {
                using (HttpClient client = new HttpClient())
                {

                    using (HttpResponseMessage response = await client.GetAsync(mod.DownloadUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                        bool canReportProgress = totalBytes != -1;

                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                        {
                            var buffer = new byte[8192];
                            long totalRead = 0;
                            int bytesRead;
                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesRead);
                                totalRead += bytesRead;
                                if (canReportProgress)
                                {
                                    int progress = (int)((totalRead * 100) / totalBytes);
                                    OnDownloadProgressChanged(modName, progress);
                                }
                            }
                        }
                    }
                }

                string extension = Path.GetExtension(downloadPath).ToLower();

                if (IsZipFile(downloadPath))
                {
                    ZipFile.ExtractToDirectory(downloadPath, extractPath);
                } else if (IsRarFile(downloadPath))
                {
                    try
                    {
                        using (var archive = SharpCompress.Archives.Rar.RarArchive.Open(downloadPath))
                        {
                            foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
                            {
                                entry.WriteToDirectory(extractPath, new SharpCompress.Common.ExtractionOptions()
                                {
                                    ExtractFullPath = true,
                                    Overwrite = true
                                });
                            }
                        }
                    } catch (Exception ex)
                    {
                        OnDownloadCompleted(modName, false, $"Error: RAR extraction failed: {ex.Message}");
                        return;
                    }

                } else
                {
                    OnDownloadCompleted(modName, false, "Error: File is not RAR or ZIP file");
                    return;
                }
                FlattenExtractedFolder(extractPath);

                var installedFiles = new List<string>();

                    string installPath = Path.Combine(_gtaPath, mod.InstallPath.TrimStart('/').Replace('/', '\\'));
                if (!Directory.Exists(installPath))
                {
                    Directory.CreateDirectory(installPath);
                }

                
                CopyDirectory(extractPath, installPath, installedFiles);

                OnDownloadCompleted(modName, true, "Installation completed successfully", installedFiles);
            }
            catch (Exception ex)
            {
                OnDownloadCompleted(modName, false, $"Fatal Error: {ex.Message}");
            }
            finally
            {
                
                if (File.Exists(downloadPath))
                {
                    try { File.Delete(downloadPath); } catch { }
                }
                if (Directory.Exists(extractPath))
                {
                    try { Directory.Delete(extractPath, true); } catch { }
                }
            }
        }

        private bool IsZipFile(string filePath)
        {
            byte[] zipSignature = new byte[] { 0x50, 0x4B, 0x03, 0x04 }; // "PK\u0003\u0004"
            byte[] fileSignature = new byte[4];
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (fileStream.Read(fileSignature, 0, 4) != 4)
                    return false;
            }
            return zipSignature.SequenceEqual(fileSignature);
        }

        private bool IsRarFile(string filePath)
        {
            byte[] rarSignature = new byte[] { 0x52, 0x61, 0x72, 0x21 }; 
            byte[] fileSignature = new byte[4];
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (fileStream.Read(fileSignature, 0, 4) != 4)
                    return false;
            }
            return rarSignature.SequenceEqual(fileSignature);
        }

        // if a folder inside a folder, it will install the folder inside, it will cut time.
        private void FlattenExtractedFolder(string extractPath)
        {
            var subdirectories = Directory.GetDirectories(extractPath);
            var files = Directory.GetFiles(extractPath);


            if (subdirectories.Length == 1 && files.Length == 0)
            {
                string nestedFolder = subdirectories[0];

                foreach (var file in Directory.GetFiles(nestedFolder))
                {
                    string destFile = Path.Combine(extractPath, Path.GetFileName(file));
                        File.Move(file, destFile);

                }
                foreach (var dir in Directory.GetDirectories(nestedFolder))
                {
                    string destDir = Path.Combine(extractPath, Path.GetFileName(dir));
                    Directory.Move(dir, destDir);
                }

                Directory.Delete(nestedFolder, true);

            }
        }

        private void CopyDirectory(string sourceDir, string destinationDir, List<string> recordFiles)
        {
            Directory.CreateDirectory(destinationDir);

            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);
                string destFile = Path.Combine(destinationDir, fileName);
                File.Copy(filePath, destFile, true);
                recordFiles.Add(destFile);
            }

            foreach (string dirPath in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(dirPath);
                string destSubDir = Path.Combine(destinationDir, dirName);
                CopyDirectory(dirPath, destSubDir, recordFiles);
            }
        }

        private void OnDownloadProgressChanged(string modName, int progressPercentage)
        {
            DownloadProgressChanged?.Invoke(this, new DownloadProgressEventArgs
            {
                ModName = modName,
                ProgressPercentage = progressPercentage
            });
        }

        private void OnDownloadCompleted(string modName, bool success, string message, List<string> installedFiles = null)
        {
            DownloadCompleted?.Invoke(this, new DownloadCompletedEventArgs
            {
                ModName = modName,
                Success = success,
                Message = message,
                InstalledFiles = installedFiles ?? new List<string>()
            });
        }
    }

    public class DownloadProgressEventArgs : EventArgs
    {
        public string ModName { get; set; }
        public int ProgressPercentage { get; set; }
    }

    public class DownloadCompletedEventArgs : EventArgs
    {
        public string ModName { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public List<string> InstalledFiles { get; set; } = new List<string>();
    }
}