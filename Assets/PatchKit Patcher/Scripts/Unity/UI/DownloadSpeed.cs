using UnityEngine;
using UnityEngine.UI;

namespace PatchKit.Patcher.Unity.UI
{
    public class DownloadSpeed : MonoBehaviour
    {
        public Text Text;

        private void Start()
        {
            PatchKit.Unity.Patcher.Patcher.Instance.UpdateAppStatusChanged += status =>
            {
                Text.text = status.IsDownloading ? (status.DownloadBytesPerSecond / 1024.0).ToString("0.0 kB/sec.") : string.Empty;
            };

            Text.text = string.Empty;
        }
    }
}