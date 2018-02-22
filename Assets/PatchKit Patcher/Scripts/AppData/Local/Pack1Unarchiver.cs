using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Ionic.Zlib;
using JetBrains.Annotations;
using PatchKit.Logging;
using PatchKit.Network;
using PatchKit.Unity.Patcher.Cancellation;
using PatchKit.Unity.Patcher.Data;
using PatchKit.Unity.Patcher.Debug;
using PatchKit.Unity.Utilities;

namespace PatchKit.Unity.Patcher.AppData.Local
{

    /// <summary>
    /// Pack1 format unarchiver.
    /// http://redmine.patchkit.net/projects/patchkit-documentation/wiki/Pack1_File_Format
    /// </summary>
    public class Pack1Unarchiver : IPack1Unarchiver
    {
        private readonly ILogger _logger;


        public event UnarchiveProgressChangedHandler UnarchiveProgressChanged;

        public Pack1Unarchiver(string packagePath, Pack1Meta metaData, string destinationDirPath, string key, string suffix = "")
            : this(packagePath, metaData, destinationDirPath, Encoding.ASCII.GetBytes(key), suffix, new BytesRange(0, -1))
        {
            // do nothing
        }

        public Pack1Unarchiver(string packagePath, Pack1Meta metaData, string destinationDirPath, string key, string suffix, BytesRange range)
            : this(packagePath, metaData, destinationDirPath, Encoding.ASCII.GetBytes(key), suffix, range)
        {
            // do nothing
        }

        public Pack1Unarchiver(string packagePath, Pack1Meta metaData, string destinationDirPath, byte[] key, string suffix, BytesRange range)
        {
            Checks.ArgumentFileExists(packagePath, "packagePath");
            Checks.ArgumentDirectoryExists(destinationDirPath, "destinationDirPath");
            Checks.ArgumentNotNull(suffix, "suffix");

            if (range.Start == 0)
            {
                Assert.AreEqual(MagicBytes.Pack1, MagicBytes.ReadFileType(packagePath), "Is not Pack1 format");
            }

            DebugLogger.LogConstructor();
            DebugLogger.LogVariable(packagePath, "packagePath");
            DebugLogger.LogVariable(destinationDirPath, "destinationDirPath");

            _packagePath = packagePath;
            _metaData = metaData;
            _destinationDirPath = destinationDirPath;
            _suffix = suffix;
            _range = range;


        }

        public void UnarchiveSingleFile(Pack1Meta.FileEntry file, CancellationToken cancellationToken, string destinationDirPath = null)
        {
            OnUnarchiveProgressChanged(file.Name, file.Type == Pack1Meta.RegularFileType, 0, 1, 0.0);

            if (!CanUnpack(file))
            {
                throw new ArgumentOutOfRangeException("file", file, null);
            }

            Unpack(file, progress => OnUnarchiveProgressChanged(file.Name, file.Type == Pack1Meta.RegularFileType, 1, 1, progress), cancellationToken, destinationDirPath);

            OnUnarchiveProgressChanged(file.Name, file.Type == Pack1Meta.RegularFileType, 0, 1, 1.0);
        }

        public void Unarchive(Pack1Info info,
            string destinationDirPath,
            string suffix,
            UnarchiveProgressChangedHandler onProgressChanged,
            out string usedSuffix,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(destinationDirPath))
            {
                throw new ArgumentException("Value cannot be null or empty.", "destinationDirPath");
            }

            if (suffix == null)
            {
                throw new ArgumentNullException("suffix");
            }

            if (onProgressChanged == null)
            {
                throw new ArgumentNullException("onProgressChanged");
            }

            try
            {
                _logger.LogDebug(string.Format("Unarchiving pack1 from {0} to {1}", info.Path, destinationDirPath));
                //TODO: Log package password here after introducing log censoring

                _logger.LogTrace("suffix = " + suffix);
                usedSuffix = suffix;

                byte[] key;
                using (var sha256 = SHA256.Create())
                {
                    key = sha256.ComputeHash(Encoding.ASCII.GetBytes(info.Password));
                }

                var iv = Convert.FromBase64String(info.Meta.Iv);

                int entry = 1;

                foreach (var file in info.Meta.Files)
                {
                    onProgressChanged(file.Name, file.Type == Pack1Meta.RegularFileType, entry, info.Meta.Files.Length, 0.0);

                    var currentFile = file;
                    var currentEntry = entry;

                    if (CanUnpack(file))
                    {
                        Unpack(file, progress =>
                        {
                            onProgressChanged(currentFile.Name, currentFile.Type == Pack1Meta.RegularFileType, currentEntry, _metaData.Files.Length, progress);
                        }, cancellationToken);
                    }
                    else
                    {
                        DebugLogger.LogWarning(string.Format("The file {0} couldn't be unpacked.", file.Name));
                    }

                    onProgressChanged(file.Name, file.Type == Pack1Meta.RegularFileType, entry, info.Meta.Files.Length, 1.0);

                    entry++;
                }

                _logger.LogDebug("Pack1 unarchived.");
            }
            catch (Exception e)
            {
                _logger.LogError("Unarchiving pack1 failed.", e);
                throw;
            }

        }

        private bool CanUnpack(BytesRange range, Pack1Meta.FileEntry file)
        {
            if (file.Type != Pack1Meta.RegularFileType)
            {
                return true;
            }

            if (range.Start == 0 && range.End == -1)
            {
                return true;
            }

            return file.Offset >= range.Start && file.Offset + file.Size <= range.End;
        }

        private void Unpack(Pack1Meta.FileEntry file, Action<double> progress, CancellationToken cancellationToken, string destinationDirPath = null)
        {
            switch (file.Type)
            {
                case Pack1Meta.RegularFileType:
                    UnpackRegularFile(file, progress, cancellationToken, destinationDirPath);
                    break;
                case Pack1Meta.DirectoryFileType:
                    progress(0.0);
                    UnpackDirectory(file);
                    progress(1.0);
                    break;
                case Pack1Meta.SymlinkFileType:
                    progress(0.0);
                    UnpackSymlink(file);
                    progress(1.0);
                    break;
                default:
                    DebugLogger.LogWarning("Unknown file type: " + file.Type);
                    break;
            }

        }

        private void UnpackDirectory(string destinationDirPath, Pack1Meta.FileEntry file)
        {
            string destPath = Path.Combine(destinationDirPath, file.Name);

            DebugLogger.Log("Creating directory " + destPath);
            Directory.CreateDirectory(destPath);
            DebugLogger.Log("Directory " + destPath + " created successfully!");
        }

        private void UnpackSymlink(string destinationDirPath, Pack1Meta.FileEntry file)
        {
            string destPath = Path.Combine(destinationDirPath, file.Name);
            DebugLogger.Log("Creating symlink: " + destPath);
            // TODO: how to create a symlink?
        }

        private void UnpackRegularFile(string pack1Path,
            string destinationDirPath,
            string suffix,
            Pack1Meta.FileEntry file,
            Action<double> onProgress,
            CancellationToken cancellationToken)
        {
            string destPath = Path.Combine(destinationDirPath, file.Name + suffix);
            DebugLogger.LogFormat("Unpacking regular file {0} to {1}", file, destPath);

            Files.CreateParents(destPath);

            var rijn = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                KeySize = 256
            };

            ICryptoTransform decryptor = rijn.CreateDecryptor(_key, _iv);

            using (var fs = new FileStream(pack1Path, FileMode.Open))
            {
                fs.Seek(file.Offset.Value - _range.Start, SeekOrigin.Begin);

                using (var limitedStream = new LimitedStream(fs, file.Size.Value))
                {
                    using (var target = new FileStream(destPath, FileMode.Create))
                    {
                        ExtractFileFromStream(limitedStream, target, file, decryptor, onProgress, cancellationToken);
                    }

                    if (Platform.IsPosix())
                    {
                        Chmod.SetMode(file.Mode.Substring(3), destPath);
                    }
                }
            }

            DebugLogger.Log("File " + file.Name + " unpacked successfully!");
        }

        private void ExtractFileFromStream(
            Stream sourceStream,
            Stream targetStream,
            Pack1Meta.FileEntry file,
            ICryptoTransform decryptor,
            Action<double> onProgress,
            CancellationToken cancellationToken)
        {
            using (var cryptoStream = new CryptoStream(sourceStream, decryptor, CryptoStreamMode.Read))
            {
                using (var gzipStream = new GZipStream(cryptoStream, CompressionMode.Decompress))
                {
                    long bytesProcessed = 0;
                    const int bufferSize = 128 * 1024;
                    var buffer = new byte[bufferSize];
                    int count;
                    while ((count = gzipStream.Read(buffer, 0, bufferSize)) != 0)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        targetStream.Write(buffer, 0, count);
                        bytesProcessed += count;
                        onProgress((double) gzipStream.Position / file.Size.Value);
                    }
                }
            }
        }
    }
}