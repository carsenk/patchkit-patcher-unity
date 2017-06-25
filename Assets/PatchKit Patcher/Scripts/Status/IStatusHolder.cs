namespace PatchKit.Patcher.Status
{
    public interface IStatusHolder
    {
        double Progress { get; }

        double Weight { get; }
    }
}