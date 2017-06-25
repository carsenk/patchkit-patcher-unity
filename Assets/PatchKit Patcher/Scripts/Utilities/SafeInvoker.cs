using System;
using PatchKit.Patcher.Debug;

namespace PatchKit.Patcher.Utilities
{
    public static class SafeInvoker
    {
        public static void Invoke(Action action, Action onSucessAction = null, Action<Exception> onFailedAction = null)
        {
            Checks.ArgumentNotNull(action, "action");
            try
            {
                action();
            }
            catch (Exception exception)
            {
                if (onFailedAction != null)
                {
                    onFailedAction(exception);
                }
            }

            if (onSucessAction != null)
            {
                onSucessAction();
            }
        }
    }
}
