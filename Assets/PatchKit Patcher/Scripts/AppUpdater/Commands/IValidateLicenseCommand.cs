namespace PatchKit.Patcher.AppUpdater.Commands
{
    public interface IValidateLicenseCommand : IAppUpdaterCommand
    {
        string KeySecret { get; }
    }
}