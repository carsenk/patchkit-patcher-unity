using JetBrains.Annotations;
using PatchKit.Unity.Patcher.Cancellation;

namespace PatchKit.Unity.Patcher.AppData.Local
{
    public interface IZipUnarchiver
    {
        void Unarchive(ZipInfo info,
            [NotNull] string destinationDirPath,
            [NotNull] string suffix,
            [NotNull] UnarchiveProgressChangedHandler onProgressChanged,
            [NotNull] out string usedSuffix,
            CancellationToken cancellationToken);
    }
}