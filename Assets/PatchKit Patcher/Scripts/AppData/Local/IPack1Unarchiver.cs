using JetBrains.Annotations;
using PatchKit.Unity.Patcher.Cancellation;

namespace PatchKit.Unity.Patcher.AppData.Local
{
    public interface IPack1Unarchiver
    {
        void Unarchive(Pack1Info info,
            [NotNull] string destinationDirPath,
            [NotNull] string suffix,
            [NotNull] UnarchiveProgressChangedHandler onProgressChanged,
            [NotNull] out string usedSuffix,
            CancellationToken cancellationToken);
    }
}