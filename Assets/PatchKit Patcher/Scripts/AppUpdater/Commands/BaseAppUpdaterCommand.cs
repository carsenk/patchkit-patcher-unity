using PatchKit.Patcher.Cancellation;
using PatchKit.Patcher.Debug;
using PatchKit.Patcher.Status;

namespace PatchKit.Patcher.AppUpdater.Commands
{
    public abstract class BaseAppUpdaterCommand : IAppUpdaterCommand
    {
        private bool _executeHasBeenCalled;
        private bool _prepareHasBeenCalled;

        public virtual void Execute(CancellationToken cancellationToken)
        {
            Assert.MethodCalledOnlyOnce(ref _executeHasBeenCalled, "Execute");
            Assert.IsTrue(_prepareHasBeenCalled, "Command not prepared.");
        }

        public virtual void Prepare(IStatusMonitor statusMonitor)
        {
            Assert.MethodCalledOnlyOnce(ref _prepareHasBeenCalled, "Prepare");
        }
    }
}