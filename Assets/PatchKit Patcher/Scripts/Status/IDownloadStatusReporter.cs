namespace PatchKit.Patcher.Status
{
    public interface IDownloadStatusReporter
    {
        void OnDownloadStarted();

        void OnDownloadProgressChanged(long bytes, long totalBytes);

        void OnDownloadEnded();
    }
}