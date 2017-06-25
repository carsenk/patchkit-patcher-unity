using PatchKit.Patcher.Cancellation;
using PatchKit.Patcher.Status;

namespace PatchKit.Patcher.AppUpdater.Commands
{
    public interface IAppUpdaterCommand
    {
        void Execute(CancellationToken cancellationToken);

        void Prepare(IStatusMonitor statusMonitor);
    }
}