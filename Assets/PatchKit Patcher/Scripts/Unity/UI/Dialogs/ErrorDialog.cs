using PatchKit.Patcher.Cancellation;
using PatchKit.Patcher.Utilities;
using UnityEngine.UI;

namespace PatchKit.Patcher.Unity.UI.Dialogs
{
    public class ErrorDialog : Dialog<ErrorDialog>
    {
        public Text ErrorText;

        public void Confirm()
        {
            OnDisplayed();
        }

        public void Display(UnityPatcherError error, CancellationToken cancellationToken)
        {
            UnityDispatcher.Invoke(() => UpdateMessage(error)).WaitOne();

            Display(cancellationToken);
        }

        private void UpdateMessage(UnityPatcherError error)
        {
            switch (error)
            {
                case UnityPatcherError.NoInternetConnection:
                    ErrorText.text = "Please check your internet connection.";
                    break;
                case UnityPatcherError.NoPermissions:
                    ErrorText.text = "Please check write permissions in application directory.";
                    break;
                case UnityPatcherError.NotEnoughDiskSpace:
                    ErrorText.text = "Not enough disk space.";
                    break;
                case UnityPatcherError.Other:
                    ErrorText.text = "Unknown error. Please try again.";
                    break;
            }
        }
    }
}
