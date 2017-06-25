using System;

namespace PatchKit.Patcher.Debug
{
    public static class EnvironmentInfo
    {
        public static string GetRuntimeVersion()
        {
            return Environment.Version.ToString();
        }

        public static string GetSystemVersion()
        {
            return Environment.OSVersion.ToString();
        }
    }
}