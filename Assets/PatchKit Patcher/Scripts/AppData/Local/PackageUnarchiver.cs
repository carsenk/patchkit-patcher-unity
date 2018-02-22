using System;
using JetBrains.Annotations;
using PatchKit.Logging;
using PatchKit.Unity.Patcher.Cancellation;

namespace PatchKit.Unity.Patcher.AppData.Local
{
    public class PackageUnarchiver : IPackageUnarchiver
    {
        private readonly ILogger _logger;
        private readonly IZipUnarchiver _zipUnarchiver;

        public PackageUnarchiver([NotNull] ILogger logger,
            [NotNull] IZipUnarchiver zipUnarchiver)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _logger = logger;
            _zipUnarchiver = zipUnarchiver;
        }

        public void Unarchive(PackageInfo info,
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
                _logger.LogDebug(string.Format("Unarchiving package from {0} to {1}", info.Path, destinationDirPath));
                _logger.LogTrace("type = " + info.Type);
                //TODO: Log package password here after introducing log censoring

                switch (info.Type)
                {
                    case PackageType.Zip:
                        _zipUnarchiver.Unarchive(new ZipInfo(info.Path, info.Password),
                            destinationDirPath, suffix,
                            onProgressChanged, out usedSuffix, cancellationToken);
                        break;
                    case PackageType.Pack1:
                        _zipUnarchiver.Unarchive(new ZipInfo(info.Path, info.Password),
                            destinationDirPath, suffix,
                            onProgressChanged, out usedSuffix, cancellationToken);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _logger.LogDebug("Package unarchived.");
            }
            catch (Exception e)
            {
                _logger.LogError("Package unarchiving failed.", e);
                throw;
            }
        }
    }
}