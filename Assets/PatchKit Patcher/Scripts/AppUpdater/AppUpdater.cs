using System;
using System.Linq;
using System.Threading;
using PatchKit.Logging;
using PatchKit.Unity.Patcher.Cancellation;
using PatchKit.Unity.Patcher.Debug;
using PatchKit.Unity.Patcher.AppUpdater.Commands;
using PatchKit.Unity.Patcher.AppUpdater.Status;

namespace PatchKit.Unity.Patcher.AppUpdater
{
    public class AppUpdater
    {
        private static readonly DebugLogger DebugLogger = new DebugLogger(typeof(AppUpdater));

        private ILogger _logger;

        public readonly AppUpdaterContext Context;

        private readonly UpdaterStatus _status = new UpdaterStatus();

        private IAppUpdaterStrategyResolver _strategyResolver;

        private bool _updateHasBeenCalled;

        public IReadOnlyUpdaterStatus Status
        {
            get { return _status; }
        }

        public AppUpdater(AppUpdaterContext context)
        {
            Assert.IsNotNull(context, "Context cannot be null");

            _logger = PatcherLogManager.DefaultLogger;

            _strategyResolver = new AppUpdaterStrategyResolver(_status);
            Context = context;
        }

        public void Update(CancellationToken cancellationToken)
        {
            Assert.MethodCalledOnlyOnce(ref _updateHasBeenCalled, "Update");

            _logger.LogDebug("Updating.");

            var commandFactory = new AppUpdaterCommandFactory();

            var checkIntegrity = commandFactory.CreateCheckVersionIntegrityCommand(Context.App.GetInstalledVersionId(), Context, false, true);
            checkIntegrity.Prepare(null);
            checkIntegrity.Execute(cancellationToken);

            if (checkIntegrity.Results.Files.Any(f => f.Status == FileIntegrityStatus.InvalidSize || f.Status == FileIntegrityStatus.MissingData))
            {
                
            }

            StrategyType type = _strategyResolver.Resolve(Context);
            var strategy = _strategyResolver.Create(type, Context);

            try
            {
                strategy.Update(cancellationToken);
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException
                    || ex is UnauthorizedAccessException
                    || ex is NotEnoughtDiskSpaceException
                    || ex is ThreadInterruptedException
                    || ex is ThreadAbortException)
                {
                    _logger.LogWarning("Strategy caused exception, to be handled further");
                    throw;
                }
                else
                {
                    _logger.LogWarning(string.Format("Strategy caused exception, being handled by fallback: {0}, Trace: {1}", ex, ex.StackTrace));

                    if (!TryHandleFallback(strategy.GetStrategyType(), cancellationToken))
                    {
                        throw;
                    }
                }
            }
        }

        private bool TryHandleFallback(StrategyType strategyType, CancellationToken cancellationToken)
        {
            var fallbackType = _strategyResolver.GetFallbackStrategy(strategyType);

            if (fallbackType == StrategyType.None)
            {
                return false;
            }

            var strategy = _strategyResolver.Create(fallbackType, Context);

            strategy.Update(cancellationToken);

            return true;
        }
    }
}