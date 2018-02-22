using JetBrains.Annotations;
using PatchKit.Unity.Patcher.Cancellation;

namespace PatchKit.Unity.Patcher.AppData.Local
{
    public interface IPackageUnarchiver
    {
        void Unarchive(PackageInfo info,
            [NotNull] string destinationDirPath,
            [NotNull] string suffix,
            [NotNull] UnarchiveProgressChangedHandler onProgressChanged,
            [NotNull] out string usedSuffix,
            CancellationToken cancellationToken);
    }
}