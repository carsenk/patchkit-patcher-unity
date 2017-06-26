namespace PatchKit.Patcher.Debug
{
    public interface IDebugLogWriter
    {
        void Log(string message);

        void LogWarning(string message);

        void LogError(string message);
    }
}
