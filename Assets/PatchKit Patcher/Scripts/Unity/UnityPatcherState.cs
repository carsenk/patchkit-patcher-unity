namespace PatchKit.Patcher.Unity
{
    public enum UnityPatcherState
    {
        None,
        LoadingPatcherData,
        LoadingPatcherConfiguration,
        WaitingForUserDecision,
        UpdatingApp,
        StartingApp
    }
}