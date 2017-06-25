namespace PatchKit.Patcher.Unity.UI.Dialogs
{
    public interface ILicenseDialog
    {
        LicenseDialogResult Display(LicenseDialogMessageType messageType);
    }
}