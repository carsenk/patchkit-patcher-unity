using PatchKit.Unity.Patcher;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PatchKit.Patcher.Unity.UI
{
    public class ProgressBar : MonoBehaviour
    {
        public Text Text;

        public Image Image;

        private void SetProgress(double progress)
        {
            Text.text = progress.ToString("0.0%");
            var anchorMax = Image.rectTransform.anchorMax;
            anchorMax.x = (float)progress;
            Image.rectTransform.anchorMax = anchorMax;
        }

        private void Start()
        {
            PatchKit.Unity.Patcher.Patcher.Instance.State.ObserveOnMainThread().Subscribe(state =>
            {
                if (state != PatcherState.UpdatingApp)
                {
                    SetProgress(1.0);
                }
            }).AddTo(this);

            PatchKit.Unity.Patcher.Patcher.Instance.UpdateAppStatusChanged += status =>
            {
                SetProgress(status.Progress);
            };

            SetProgress(1.0);
        }
    }
}