namespace PatchKit.Patcher.Status
{
    public interface IGeneralStatusReporter
    {
        void OnProgressChanged(double progress);
    }
}