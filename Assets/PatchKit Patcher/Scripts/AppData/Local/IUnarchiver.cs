using PatchKit.Patcher.Cancellation;

namespace PatchKit.Patcher.AppData.Local
{
    public interface IUnarchiver
    {
        event UnarchiveProgressChangedHandler UnarchiveProgressChanged;

        void Unarchive(CancellationToken cancellationToken);
    }
}