using System;
using Ionic.Zip;
using JetBrains.Annotations;
using PatchKit.Logging;
using PatchKit.Unity.Patcher.Cancellation;

namespace PatchKit.Unity.Patcher.AppData.Local
{
    /// <summary>
    /// Zip unarchiver.
    /// </summary>
    public class ZipUnarchiver : IZipUnarchiver
    {
        private readonly ILogger _logger;

        public ZipUnarchiver([NotNull] ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _logger = logger;
        }

        public void Unarchive(ZipInfo info,
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
                _logger.LogDebug(string.Format("Unarchiving zip from {0} to {1}", info.Path, destinationDirPath));
                //TODO: Log package password here after introducing log censoring

                _logger.LogDebug(string.Format(
                    "Requested suffix equals '{0}' but this zip unarchiver doesn't support suffixes so it won't be used.",
                    suffix));

                usedSuffix = string.Empty;

                using (var zip = ZipFile.Read(info.Path))
                {
                    zip.Password = info.Password;

                    int entry = 1;

                    foreach (var zipEntry in zip)
                    {
                        onProgressChanged(zipEntry.FileName, !zipEntry.IsDirectory, entry, zip.Count, 0.0);

                        cancellationToken.ThrowIfCancellationRequested();

                        _logger.LogDebug(string.Format("Unarchving entry {0}/{1}...", entry, zip.Count));
                        _logger.LogTrace("entryName = " + zipEntry.FileName);

                        zipEntry.Extract(destinationDirPath, ExtractExistingFileAction.OverwriteSilently);

                        _logger.LogDebug(string.Format("Entry {0}/{1} unarchived.", entry, zip.Count));

                        onProgressChanged(zipEntry.FileName, !zipEntry.IsDirectory, entry, zip.Count, 1.0);

                        entry++;
                    }
                }

                _logger.LogDebug("Zip unarchived.");
            }
            catch (Exception e)
            {
                _logger.LogError("Unarchving zip failed.", e);
                throw;
            }

        }
    }
}