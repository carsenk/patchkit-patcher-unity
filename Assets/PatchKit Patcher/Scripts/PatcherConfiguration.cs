using System;
using PatchKit.Patcher.AppUpdater;

namespace PatchKit.Unity.Patcher
{
    [Serializable]
    public struct PatcherConfiguration
    {
        public AppUpdaterConfiguration AppUpdaterConfiguration;

        public bool AutomaticallyStartApp;

        public bool AutomaticallyCheckForAppUpdates;

        public bool AutomaticallyInstallApp;
    }
}