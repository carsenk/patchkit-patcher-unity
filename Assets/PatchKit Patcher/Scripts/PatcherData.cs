using System;

namespace PatchKit.Patcher
{
    [Serializable]
    public struct PatcherData
    {
        public string AppSecret;

        public string AppDataPath;

        public int OverrideLatestVersionId;
    }
}