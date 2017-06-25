using System;

namespace PatchKit.Patcher.AppData.Remote.Downloaders
{
    public class DownloadedResourceValidationException : Exception
    {
        public DownloadedResourceValidationException(string message) : base(message)
        {
        }
    }
}