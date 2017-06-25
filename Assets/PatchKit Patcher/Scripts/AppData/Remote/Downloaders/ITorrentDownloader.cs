using System;
using PatchKit.Patcher.Cancellation;

namespace PatchKit.Patcher.AppData.Remote.Downloaders
{
    public interface ITorrentDownloader : IDisposable
    {
        event DownloadProgressChangedHandler DownloadProgressChanged;

        void Download(CancellationToken cancellationToken);
    }
}