using System;

namespace PatchKit.Patcher.AppUpdater
{
    [Serializable]
    public struct AppUpdaterConfiguration
    {
        public bool UseTorrents;

        public bool CheckConsistencyBeforeDiffUpdate;
    }
}