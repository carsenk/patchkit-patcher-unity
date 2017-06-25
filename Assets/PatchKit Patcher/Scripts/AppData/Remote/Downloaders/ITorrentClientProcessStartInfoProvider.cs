using System.Diagnostics;

namespace PatchKit.Patcher.AppData.Remote.Downloaders
{
    public interface ITorrentClientProcessStartInfoProvider
    {
        ProcessStartInfo GetProcessStartInfo();
    }
}
