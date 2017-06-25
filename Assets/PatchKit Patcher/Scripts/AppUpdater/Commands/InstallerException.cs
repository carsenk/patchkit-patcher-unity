using System;

namespace PatchKit.Patcher.AppUpdater.Commands
{
    public class InstallerException : Exception
    {
        public InstallerException(string message) : base(message)
        {
        }
    }
}