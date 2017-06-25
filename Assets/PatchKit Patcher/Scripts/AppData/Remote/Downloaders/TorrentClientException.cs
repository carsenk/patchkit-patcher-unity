using System;

namespace PatchKit.Patcher.AppData.Remote.Downloaders
{
    public class TorrentClientException : Exception
    {
        public TorrentClientException(string message) : base(message)
        {
        }
    }
}