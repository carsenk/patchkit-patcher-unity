using PatchKit.Unity.Patcher.Cancellation;

namespace PatchKit.Patcher.AppUpdater
{
    public interface IAppUpdaterStrategy
    {
        void Update(CancellationToken cancellationToken);
    }
}