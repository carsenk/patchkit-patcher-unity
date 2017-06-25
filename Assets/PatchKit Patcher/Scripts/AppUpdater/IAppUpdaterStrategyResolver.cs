namespace PatchKit.Patcher.AppUpdater
{
    public interface IAppUpdaterStrategyResolver
    {
        IAppUpdaterStrategy Resolve(AppUpdaterContext context);
    }
}