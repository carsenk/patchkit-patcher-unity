using PatchKit.Patcher.Cancellation;
using PatchKit.Patcher.Debug;

namespace PatchKit.Patcher.AppUpdater
{
    public class AppUpdaterEmptyStrategy : IAppUpdaterStrategy
    {
        private static readonly DebugLogger DebugLogger = new DebugLogger(typeof(AppUpdaterStrategyResolver));

        public void Update(CancellationToken cancellationToken)
        {
            DebugLogger.Log("Updating with empty strategy. Doing nothing. ");
        }
    }
}