using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PatchKit.Patcher.Unity.UI
{
    public class Status : MonoBehaviour
    {
        public Text Text;

        private void Start()
        {
            UnityPatcher.Instance.State.ObserveOnMainThread().Subscribe(state =>
            {
                switch (state)
                {
                    case UnityPatcherState.None:
                        Text.text = string.Empty;
                        break;
                    case UnityPatcherState.LoadingPatcherData:
                        Text.text = "Loading data...";
                        break;
                    case UnityPatcherState.LoadingPatcherConfiguration:
                        Text.text = "Loading configuration...";
                        break;
                    case UnityPatcherState.WaitingForUserDecision:
                        Text.text = string.Empty;
                        break;
                    case UnityPatcherState.StartingApp:
                        Text.text = "Starting application...";
                        break;
                    case UnityPatcherState.UpdatingApp:
                        Text.text = "Updating application...";
                        break;
                }
            }).AddTo(this);
        }
    }
}